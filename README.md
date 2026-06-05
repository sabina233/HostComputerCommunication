# 上位机通信工具集

基于 .NET 10 的上位机通信调试工具，涵盖串口、Modbus、TCP/Socket、OPC UA 四大通信模块，适用于工业自动化设备调试和上位机开发学习。

## 项目结构

```
HostComputerCommunication/
├── HostComputerCommunication.Common/          # 公共工具类
│   ├── Helpers/
│   │   ├── ByteHelper.cs                     # 字节操作（大小端转换、Hex转换）
│   │   ├── CrcHelper.cs                      # CRC16 Modbus 校验
│   │   └── Logger.cs                         # 日志记录器
│   └── Models/
│       └── CommunicationConfig.cs            # 通信配置模型
├── HostComputerCommunication.SerialPort/      # 串口通信模块
│   ├── ISerialPort.cs                        # 串口接口（支持真实串口和模拟器）
│   ├── SerialPortManager.cs                  # 串口管理器
│   └── SerialPortSimulator.cs                # 串口模拟器（无实物设备时自测）
├── HostComputerCommunication.Modbus/          # Modbus 协议模块
│   ├── Protocol/
│   │   ├── ModbusFrameBuilder.cs             # Modbus 帧构建器（RTU/TCP）
│   │   └── ModbusResponseParser.cs           # Modbus 响应解析器
│   ├── ModbusRtuClient.cs                    # Modbus RTU 客户端（串口通信）
│   └── ModbusTcpClient.cs                    # Modbus TCP 客户端（以太网通信）
├── HostComputerCommunication.TcpSocket/       # TCP/Socket 通信模块
│   ├── TcpClientManager.cs                   # TCP 客户端（心跳/断线重连）
│   └── TcpServerManager.cs                   # TCP 服务端（多客户端管理）
├── HostComputerCommunication.OpcUa/           # OPC UA 通信模块
│   └── OpcUaClient.cs                        # OPC UA 客户端
├── HostComputerCommunication.UI/              # WinForms 主界面
│   ├── Form1.cs                              # 主窗体（TabControl 切换模块）
│   └── Controls/
│       ├── SerialPortControl.cs/.Designer.cs  # 串口调试助手界面
│       ├── ModbusControl.cs/.Designer.cs      # Modbus 调试工具界面
│       ├── TcpSocketControl.cs/.Designer.cs   # TCP 通信工具界面
│       └── OpcUaControl.cs/.Designer.cs       # OPC UA 客户端界面
└── HostComputerCommunication.Tests/           # 单元测试
    ├── ByteHelperTests.cs
    ├── CrcHelperTests.cs
    ├── ModbusFrameBuilderTests.cs
    └── ModbusResponseParserTests.cs
```

## 环境要求

- **操作系统**: Windows 10/11
- **SDK**: .NET 10 SDK
- **IDE**: Visual Studio 2022 17.10+ 或 Rider 2024+
- **可选仿真工具**:
  - com0com（虚拟串口对）
  - ModRSsim2（Modbus 从站模拟器）
  - Prosys OPC UA Simulation Server（OPC UA 模拟服务器）

## 快速开始

### 编译运行

```bash
# 克隆项目后，在项目根目录执行
dotnet build

# 启动主程序
dotnet run --project HostComputerCommunication.UI
```

### 运行测试

```bash
dotnet test
```

---

## 模块一：串口调试助手

### 功能概述

串口调试助手用于 RS232/RS485 串口通信调试，支持 Hex 和 ASCII 两种数据格式，带收发日志和字节统计。

### 界面说明

| 区域 | 说明 |
|------|------|
| **连接配置** | 串口选择、波特率（9600~115200）、数据位（7/8）、校验位（None/Odd/Even/Mark/Space）、停止位（1/1.5/2） |
| **发送区** | Hex/ASCII 模式切换，支持 Ctrl+Enter 快捷发送 |
| **接收区** | 颜色区分收发（青色=发送，绿色=接收），可显示时间戳，自动滚动 |
| **系统日志** | 分级显示（Debug/Info/Warning/Error） |

### 操作步骤

1. **选择串口**: 在"串口"下拉框中选择目标串口，点击"刷新"可更新列表
2. **配置参数**: 设置波特率、数据位、校验位、停止位（两端设备参数必须一致）
3. **打开串口**: 点击"打开串口"按钮，状态变为"已连接"（绿色）
4. **发送数据**:
   - Hex 模式: 输入十六进制字符串，如 `01 03 00 00 00 01`
   - ASCII 模式: 输入文本内容，如 `Hello`
   - 点击"发送"或按 Ctrl+Enter
5. **查看接收**: 接收区实时显示收到的数据
6. **关闭串口**: 点击"关闭串口"

### 模拟模式

勾选"模拟模式"后，无需真实串口设备即可测试。模拟器支持三种模式：

| 模式 | 说明 |
|------|------|
| **回环模式** | 发送的数据原样返回，用于验证基本通信流程 |
| **随机响应** | 返回随机长度的随机数据，用于测试解析逻辑 |
| **Modbus 从站** | 按 Modbus RTU 协议返回寄存器数据，用于测试 Modbus 通信 |

### 日志导出

点击"导出日志"可将接收区内容保存为 txt 文件，文件名格式：`SerialLog_20260605_143000.txt`

---

## 模块二：Modbus 调试工具

### 功能概述

Modbus 调试工具支持 Modbus RTU（串口）和 Modbus TCP（以太网）两种通信方式，支持所有常用功能码，可按多种数据类型解析寄存器数据。

### 支持的功能码

| 功能码 | 名称 | 读/写 | 说明 |
|--------|------|-------|------|
| 0x01 | 读线圈 | 读 | 读取线圈状态（ON/OFF） |
| 0x02 | 读离散输入 | 读 | 读取离散输入状态 |
| 0x03 | 读保持寄存器 | 读 | 读取保持寄存器值（最常用） |
| 0x04 | 读输入寄存器 | 读 | 读取输入寄存器值 |
| 0x05 | 写单个线圈 | 写 | 控制单个线圈 ON/OFF |
| 0x06 | 写单个寄存器 | 写 | 写入单个寄存器值 |
| 0x15 | 写多个线圈 | 写 | 批量控制线圈 |
| 0x16 | 写多个寄存器 | 写 | 批量写入寄存器值 |

### 数据类型解析

| 类型 | 说明 | 示例 |
|------|------|------|
| UInt16 | 无符号16位整数 | 65535 |
| Int16 | 有符号16位整数 | -32768 ~ 32767 |
| UInt32 | 无符号32位整数（占2个寄存器） | 4294967295 |
| Float32 | 32位浮点数（占2个寄存器） | 3.14159 |
| Hex | 十六进制显示 | 0xFFFF |
| ASCII | ASCII 字符 | "Hi" |

### 操作步骤

#### Modbus RTU 模式

1. 选择"RTU"单选按钮
2. 选择串口和波特率
3. 设置从站地址（默认 1）
4. 点击"连接"
5. 选择功能码（如"03 - 读保持寄存器"）
6. 设置起始地址和数量
7. 点击"读取"

#### Modbus TCP 模式

1. 选择"TCP"单选按钮
2. 输入服务器 IP 和端口（默认 127.0.0.1:502）
3. 设置从站地址
4. 点击"连接"
5. 后续操作同 RTU 模式

#### 写入操作

1. 选择写入功能码（05/06/16）
2. 设置起始地址
3. 在"写入值"输入框中输入值
   - 单个值: `100`
   - 多个值（逗号分隔）: `100,200,300`
4. 点击"写入"

### 模拟模式

勾选"模拟模式"后，Modbus RTU 会使用内置的 Modbus 从站模拟器，返回随机寄存器数据。

### Modbus 报文格式

#### RTU 帧结构

```
[从站地址][功能码][数据...][CRC低][CRC高]
  1字节     1字节   N字节    1字节   1字节
```

#### TCP 帧结构 (MBAP Header)

```
[事务ID][协议ID][长度][单元ID][功能码][数据...]
 2字节   2字节  2字节  1字节   1字节   N字节
```

---

## 模块三：TCP/Socket 通信工具

### 功能概述

TCP 通信工具支持客户端和服务端两种模式，支持 Hex 和 ASCII 数据格式，适用于自定义协议调试。

### 客户端模式

用于连接远程 TCP 服务器。

**操作步骤：**

1. 选择"客户端"单选按钮
2. 输入服务器 IP 和端口
3. 勾选"自动重连"（断线后自动重连）
4. 点击"连接"
5. 在发送区输入数据并发送
6. 接收区显示服务器返回的数据

**特性：**
- 自动心跳检测
- 断线自动重连（可配置）
- 连接状态实时显示

### 服务端模式

用于监听客户端连接，适用于模拟 TCP 服务器或调试客户端程序。

**操作步骤：**

1. 选择"服务端"单选按钮
2. 设置监听端口
3. 点击"启动"
4. 等待客户端连接
5. 在"已连接客户端"列表中选择目标客户端
6. 发送数据给选定客户端

**特性：**
- 支持多客户端同时连接
- 客户端列表实时更新
- 可向指定客户端发送数据

---

## 模块四：OPC UA 客户端

### 功能概述

OPC UA 客户端用于连接 OPC UA 服务器，支持节点浏览、数据读写和订阅监控。

### 操作步骤

#### 连接服务器

1. 输入 OPC UA 服务器端点 URL（如 `opc.tcp://localhost:4840`）
2. 如需认证，输入用户名和密码
3. 点击"连接"
4. 状态变为"已连接"（绿色）

#### 浏览节点

1. 连接成功后，点击"刷新"
2. 树形控件显示 ObjectsFolder 下的子节点
3. 点击节点可自动填充"节点ID"输入框

#### 读取数据

1. 在"节点ID"输入框中输入节点标识（如 `ns=2;i=1`）
2. 点击"读取"
3. "值"输入框显示读取结果

#### 写入数据

1. 输入节点ID
2. 在"值"输入框中输入要写入的值
3. 点击"写入"

#### 订阅监控

1. 输入节点ID
2. 点击"订阅"
3. "订阅数据"表格中新增一行，实时更新节点值

### OPC UA 节点ID格式

| 格式 | 示例 | 说明 |
|------|------|------|
| 数字ID | `ns=2;i=1` | 命名空间2，数字标识1 |
| 字符串ID | `ns=2;s=Temperature` | 命名空间2，字符串标识 |
| GUID | `ns=2;g=12345678-...` | 命名空间2，GUID标识 |

---

## 技术要点

### 串口通信

- 使用 `System.IO.Ports.SerialPort` 类
- 注意线程安全：`DataReceived` 事件在非 UI 线程触发，需 `Invoke` 回 UI 线程
- 串口参数两端必须一致（波特率、数据位、校验位、停止位）

### Modbus 协议

- **CRC16 校验**: RTU 模式必须校验，使用多项式 0xA001
- **字节序**: Modbus 使用大端序（高字节在前）
- **超时重试**: 建议超时 1000ms，重试 3 次
- **功能码异常**: 异常响应功能码 = 原功能码 + 0x80

### TCP/Socket

- **粘包处理**: 本项目使用同步读取，每次读取缓冲区全部数据
- **心跳机制**: 客户端定时发送空数据保持连接
- **断线重连**: 检测到连接断开后自动重连

### OPC UA

- 使用 OPC Foundation 官方 .NET Standard 库
- 异步 API：所有读写操作均为异步
- 订阅机制：通过 `Subscription` + `MonitoredItem` 实现数据变更推送

---

## 常见问题

### Q: 串口打开失败

**A:** 检查以下几点：
1. 串口是否被其他程序占用
2. 串口名称是否正确（Windows 设备管理器查看）
3. USB 转串口驱动是否安装

### Q: Modbus 通信无响应

**A:** 检查以下几点：
1. 从站地址是否正确
2. 串口参数是否与设备一致
3. 波特率是否匹配
4. RS485 接线是否正确（A+/B- 不要接反）
5. 可先用模拟模式测试程序逻辑

### Q: TCP 连接失败

**A:** 检查以下几点：
1. 服务器 IP 和端口是否正确
2. 防火墙是否放行对应端口
3. 服务器是否已启动并监听

### Q: OPC UA 连接失败

**A:** 检查以下几点：
1. OPC UA 服务器是否运行
2. 端点 URL 是否正确
3. 安全策略是否匹配
4. 可使用 Prosys OPC UA Simulation Server 进行测试

---

## 扩展建议

### 已实现

- 串口调试助手（含模拟器）
- Modbus RTU/TCP 调试工具
- TCP 客户端/服务端
- OPC UA 客户端
- 单元测试（32个）

### 可扩展

1. **数据存储**: 集成 SQLite/InfluxDB 存储采集数据
2. **趋势图表**: 使用 LiveCharts2 绘制实时数据曲线
3. **报警管理**: 设定阈值，超限报警
4. **PLC 对接**: 集成 S7NetPlus 对接西门子 PLC
5. **WPF 迁移**: 将 WinForms 界面迁移到 WPF/MVVM
6. **MQTT 支持**: 添加 MQTT 客户端用于物联网场景
7. **协议自定义**: 实现自定义协议的编解码框架

---

## 构建和发布

```bash
# Debug 构建
dotnet build

# Release 构建
dotnet build -c Release

# 发布为独立可执行文件（无需安装 .NET 运行时）
dotnet publish HostComputerCommunication.UI -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true

# 发布为依赖框架的可执行文件（需要安装 .NET 10 运行时）
dotnet publish HostComputerCommunication.UI -c Release -r win-x64 --self-contained false
```

---

## 许可证

本项目仅供学习和内部使用。
