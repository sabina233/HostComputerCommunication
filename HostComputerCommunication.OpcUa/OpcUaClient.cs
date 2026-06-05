using HostComputerCommunication.Common.Helpers;
using HostComputerCommunication.Common.Models;
using Opc.Ua;
using Opc.Ua.Client;
using Opc.Ua.Configuration;

namespace HostComputerCommunication.OpcUa;

/// <summary>
/// OPC UA 客户端
/// </summary>
public class OpcUaClient : IDisposable
{
    private ISession? _session;
    private readonly Logger _logger;
    private readonly OpcUaConfig _config;
    private bool _disposed;

    public event EventHandler<bool>? ConnectionStateChanged;
    public event EventHandler<DataValueChangedEventArgs>? DataValueChanged;

    public bool IsConnected => _session?.Connected ?? false;
    public ISession? Session => _session;

    public OpcUaClient(Logger logger, OpcUaConfig config)
    {
        _logger = logger;
        _config = config;
    }

    /// <summary>
    /// 连接到 OPC UA 服务器
    /// </summary>
    public async Task<bool> ConnectAsync()
    {
        try
        {
            var applicationConfig = new ApplicationConfiguration
            {
                ApplicationName = "HostComputerCommunication",
                ApplicationUri = $"urn:{Utils.GetHostName()}:HostComputerCommunication",
                ApplicationType = ApplicationType.Client,
                SecurityConfiguration = new SecurityConfiguration
                {
                    AutoAcceptUntrustedCertificates = _config.AutoAccept,
                    ApplicationCertificate = new CertificateIdentifier
                    {
                        StoreType = "Directory",
                        StorePath = "CertificateStores/MachineDefault"
                    }
                },
                TransportQuotas = new TransportQuotas
                {
                    OperationTimeout = _config.SessionTimeout
                },
                ClientConfiguration = new ClientConfiguration
                {
                    DefaultSessionTimeout = _config.SessionTimeout
                }
            };

            await applicationConfig.ValidateAsync(ApplicationType.Client);

            var sessionFactory = new DefaultSessionFactory();
            var endpoint = CoreClientUtils.SelectEndpoint(applicationConfig, _config.EndpointUrl, false);
            var configuredEndpoint = new ConfiguredEndpoint(null, endpoint, EndpointConfiguration.Create(applicationConfig));

            _session = await sessionFactory.CreateAsync(
                applicationConfig,
                configuredEndpoint,
                false,
                "HostComputerCommunication",
                (uint)_config.SessionTimeout,
                GetUserIdentity(),
                null);

            _logger.Info($"已连接到 OPC UA 服务器: {_config.EndpointUrl}", nameof(OpcUaClient));
            ConnectionStateChanged?.Invoke(this, true);
            return true;
        }
        catch (Exception ex)
        {
            _logger.Error($"连接 OPC UA 服务器失败: {ex.Message}", nameof(OpcUaClient));
            return false;
        }
    }

    /// <summary>
    /// 断开连接
    /// </summary>
    public async Task DisconnectAsync()
    {
        try
        {
            if (_session != null)
            {
                await _session.CloseAsync();
                _session.Dispose();
                _session = null;
            }

            _logger.Info("已断开 OPC UA 连接", nameof(OpcUaClient));
            ConnectionStateChanged?.Invoke(this, false);
        }
        catch (Exception ex)
        {
            _logger.Error($"断开 OPC UA 连接异常: {ex.Message}", nameof(OpcUaClient));
        }
    }

    /// <summary>
    /// 浏览节点
    /// </summary>
    public async Task<BrowseResultCollection> BrowseAsync(NodeId nodeId)
    {
        if (_session == null)
            throw new InvalidOperationException("未连接到 OPC UA 服务器");

        var browseDescription = new BrowseDescription
        {
            NodeId = nodeId,
            BrowseDirection = BrowseDirection.Forward,
            ReferenceTypeId = ReferenceTypeIds.HierarchicalReferences,
            IncludeSubtypes = true,
            NodeClassMask = (uint)(NodeClass.Object | NodeClass.Variable),
            ResultMask = (uint)BrowseResultMask.All
        };

        var response = await _session.BrowseAsync(
            null,
            null,
            0,
            new BrowseDescriptionCollection { browseDescription },
            CancellationToken.None);

        return response.Results;
    }

    /// <summary>
    /// 读取节点值
    /// </summary>
    public async Task<DataValue> ReadAsync(NodeId nodeId)
    {
        if (_session == null)
            throw new InvalidOperationException("未连接到 OPC UA 服务器");

        var response = await _session.ReadAsync(
            null,
            0,
            TimestampsToReturn.Both,
            new ReadValueIdCollection { new ReadValueId { NodeId = nodeId, AttributeId = Attributes.Value } },
            CancellationToken.None);

        return response.Results[0];
    }

    /// <summary>
    /// 写入节点值
    /// </summary>
    public async Task<bool> WriteAsync(NodeId nodeId, object value)
    {
        if (_session == null)
            throw new InvalidOperationException("未连接到 OPC UA 服务器");

        var response = await _session.WriteAsync(
            null,
            new WriteValueCollection
            {
                new WriteValue
                {
                    NodeId = nodeId,
                    AttributeId = Attributes.Value,
                    Value = new DataValue(new Variant(value))
                }
            },
            CancellationToken.None);

        return StatusCode.IsGood(response.Results[0]);
    }

    /// <summary>
    /// 创建订阅
    /// </summary>
    public async Task<Subscription> CreateSubscriptionAsync(uint publishingInterval = 1000)
    {
        if (_session == null)
            throw new InvalidOperationException("未连接到 OPC UA 服务器");

        var subscription = new Subscription(_session.DefaultSubscription)
        {
            PublishingEnabled = true,
            PublishingInterval = (int)publishingInterval
        };

        _session.AddSubscription(subscription);
        await subscription.CreateAsync();
        return subscription;
    }

    /// <summary>
    /// 添加监控项
    /// </summary>
    public async Task<MonitoredItem> AddMonitoredItemAsync(Subscription subscription, NodeId nodeId, string displayName)
    {
        var monitoredItem = new MonitoredItem(subscription.DefaultItem, true)
        {
            StartNodeId = nodeId,
            AttributeId = Attributes.Value,
            DisplayName = displayName,
            SamplingInterval = 1000
        };

        monitoredItem.Notification += OnNotification;
        subscription.AddItem(monitoredItem);
        await subscription.ApplyChangesAsync();
        return monitoredItem;
    }

    private void OnNotification(MonitoredItem item, MonitoredItemNotificationEventArgs e)
    {
        if (e.NotificationValue is DataValue dataValue)
        {
            _logger.Debug($"节点 {item.DisplayName} 值变更: {dataValue.Value}", nameof(OpcUaClient));
            DataValueChanged?.Invoke(this, new DataValueChangedEventArgs(item.DisplayName, dataValue));
        }
    }

    private UserIdentity GetUserIdentity()
    {
        if (!string.IsNullOrEmpty(_config.Username))
        {
            return new UserIdentity
            {
                DisplayName = _config.Username
            };
        }
        return new UserIdentity();
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            DisconnectAsync().GetAwaiter().GetResult();
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }
}

/// <summary>
/// 数据值变更事件参数
/// </summary>
public class DataValueChangedEventArgs : EventArgs
{
    public string DisplayName { get; }
    public DataValue DataValue { get; }

    public DataValueChangedEventArgs(string displayName, DataValue dataValue)
    {
        DisplayName = displayName;
        DataValue = dataValue;
    }
}
