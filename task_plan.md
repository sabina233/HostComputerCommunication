# Task Plan: 上位机软件工程师转型项目规划

## Goal
搭建一个上位机通信工具集，包含串口调试助手、Modbus 调试工具、TCP/Socket 通信框架（复习巩固）和 OPC UA 客户端（新技能学习），为转型上位机软件工程师做项目储备。

## Current Phase
All Complete

## Phases

### Phase 1: 需求分析与技术选型
- [x] 了解用户背景（2年 C#、WinForms、ASP.NET Core）
- [x] 明确转型目标（上位机软件工程师）
- [x] 确定项目范围：4个子项目（串口+Modbus+TCP复习，OPC UA新学）
- [x] 选定技术栈和框架（.NET 10、WinForms、NModbus4、OPC UA .NET Standard）
- **Status:** complete

### Phase 2: 项目结构搭建
- [x] 创建解决方案 HostComputerCommunication
- [x] 创建子项目结构（每个通信模块一个类库）
- [x] 引入 NuGet 包（System.IO.Ports、OPC Foundation UA .NET Standard）
- [x] 设计分层架构（通信层、协议层、业务层、UI层）
- **Status:** complete

### Phase 3: 串口调试助手（复习）
- [x] 串口管理器（SerialPortManager）：打开/关闭/参数配置
- [x] 数据收发：Hex/ASCII 模式切换
- [x] 收发日志和时间戳
- [x] 异常处理和连接状态管理
- [x] 串口模拟器（SerialPortSimulator，3种模式：回环/随机/Modbus从站）
- [x] WinForms 串口调试界面（SerialPortControl）
- **Status:** complete

### Phase 4: Modbus 调试工具（复习）
- [x] Modbus RTU（串口通信，CRC16 校验）
- [x] Modbus TCP（以太网通信，MBAP 头）
- [x] 常用功能码：01/02/03/04/05/06/15/16
- [x] 报文解析和构建工具类（ModbusFrameBuilder、ModbusResponseParser）
- [x] 寄存器数据展示（支持多种数据类型：short/float/string）
- [x] WinForms Modbus 调试界面
- **Status:** complete

### Phase 5: TCP/Socket 通信框架（复习）
- [x] TCP 客户端/服务端实现（TcpClientManager + TcpServerManager）
- [x] 心跳检测机制
- [x] 断线重连逻辑
- [x] WinForms TCP 调试界面（客户端/服务端双模式）
- **Status:** complete

### Phase 6: OPC UA 客户端（新技能）
- [x] OPC UA 连接管理（连接/断开/会话管理）
- [x] 节点浏览（Browse）
- [x] 数据读写（Read/Write）
- [x] 订阅机制（Subscription/MonitoredItem）
- [x] WinForms OPC UA 调试界面
- **Status:** complete

### Phase 7: UI 界面整合
- [x] WinForms 主界面（模块切换 TabControl）
- [x] 串口调试工具界面
- [x] Modbus 调试工具界面
- [x] TCP 调试工具界面
- [x] OPC UA 客户端界面
- **Status:** complete

### Phase 8: 测试与文档
- [x] 单元测试（CRC校验、Modbus帧构建、Modbus响应解析、字节操作，共32个测试全部通过）
- [x] 集成测试（模拟设备通信，通过 SerialPortSimulator 的 Modbus 从站模拟模式）
- **Status:** complete

## Key Questions
1. OPC UA 是否需要对接具体 PLC？（可先用模拟服务器测试）
2. 是否需要支持 WPF？（建议先用 WinForms，后续可迁移）
3. 子项目是否需要独立可运行？（建议每个模块既能独立运行调试，也能整合到主程序）

## Decisions Made
| Decision | Rationale |
|----------|-----------|
| 使用 .NET 10 | 长期支持版本，性能好 |
| 先 WinForms 后 WPF | 用户已有 WinForms 经验，降低学习曲线 |
| 模块化设计（每个通信模块独立类库） | 可单独使用和测试，也方便整合 |
| 串口/Modbus/TCP 当复习做 | 用户已有基础，快速过一遍即可 |
| OPC UA 重点学习 | 工业 4.0 标准，含金量高 |

## Errors Encountered
| Error | Attempt | Resolution |
|-------|---------|------------|
|       |         |            |

## Notes
- 更新阶段状态：pending → in_progress → complete
- 重大决策前重新读取计划文件
- 记录所有错误以避免重复
