using System.Threading.Channels;

namespace HostComputerCommunication.Common.Helpers;

/// <summary>
/// 日志级别枚举
/// </summary>
public enum LogLevel
{
    Debug,    // 调试信息
    Info,     // 普通信息
    Warning,  // 警告
    Error     // 错误
}

/// <summary>
/// 日志事件参数
/// </summary>
public class LogEventArgs : EventArgs
{
    /// <summary>日志时间戳</summary>
    public DateTime Timestamp { get; }

    /// <summary>日志级别</summary>
    public LogLevel Level { get; }

    /// <summary>日志消息内容</summary>
    public string Message { get; }

    /// <summary>日志来源（类名）</summary>
    public string? Source { get; }

    public LogEventArgs(LogLevel level, string message, string? source = null)
    {
        Timestamp = DateTime.Now;
        Level = level;
        Message = message;
        Source = source;
    }
}

/// <summary>
/// 异步日志记录器
/// 使用 Channel 缓冲日志事件，避免大量日志写入时阻塞 UI 线程
/// </summary>
public class Logger
{
    /// <summary>日志事件，UI 控件订阅此事件来显示日志</summary>
    public event EventHandler<LogEventArgs>? LogReceived;

    private readonly Channel<LogEventArgs> _channel =
        Channel.CreateUnbounded<LogEventArgs>();

    /// <summary>初始化异步日志消费循环</summary>
    public Logger()
    {
        Task.Run(ProcessLogQueueAsync);
    }

    /// <summary>记录调试级别日志</summary>
    public void Debug(string message, string? source = null)
        => Log(LogLevel.Debug, message, source);

    /// <summary>记录普通信息日志</summary>
    public void Info(string message, string? source = null)
        => Log(LogLevel.Info, message, source);

    /// <summary>记录警告级别日志</summary>
    public void Warning(string message, string? source = null)
        => Log(LogLevel.Warning, message, source);

    /// <summary>记录错误级别日志</summary>
    public void Error(string message, string? source = null)
        => Log(LogLevel.Error, message, source);

    private void Log(LogLevel level, string message, string? source)
    {
        _channel.Writer.TryWrite(new LogEventArgs(level, message, source));
    }

    private async Task ProcessLogQueueAsync()
    {
        await foreach (var args in _channel.Reader.ReadAllAsync())
        {
            LogReceived?.Invoke(this, args);
        }
    }
}
