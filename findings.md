# Findings & Decisions

## Requirements
- 用户背景：近2年 C# 开发经验，熟悉 WinForms 和 ASP.NET Core
- 目标：转型为上位机软件工程师
- 项目范围（已确定）：
  - 复习巩固：串口调试助手 + Modbus 调试工具 + TCP/Socket 通信框架
  - 新技能：OPC UA 客户端
- 用户已有串口/Modbus/TCP 基础，这三个当复习快速过

## Research Findings

### 上位机核心技术栈
- **通信协议**：Modbus RTU/TCP（工业标准）、OPC UA（新一代标准）、MQTT（物联网场景）
- **硬件接口**：串口（RS232/RS485）、网口（TCP/IP）、CAN 总线
- **PLC 品牌**：西门子（S7-1200/1500，市占率最高）、三菱（FX/Q 系列）、欧姆龙、台达
- **数据存储**：时序数据库（InfluxDB、TDengine）用于高频数据；SQLite/MySQL 用于业务数据

### 上位机工程师核心技能
1. 串口编程（System.IO.Ports）
2. TCP/Socket 编程
3. Modbus 协议（RTU + TCP）
4. 多线程/异步编程（设备通信是并发场景）
5. 数据解析（字节操作、大小端转换）
6. WPF/MVVM（行业趋势，逐步替代 WinForms）
7. OPC UA（工业 4.0 标准）

### 面试高频考点
- 串口通信参数配置和调试
- Modbus 协议报文结构和 CRC 校验
- 多设备并发通信的线程安全
- 通信超时和重连机制
- 大小端字节序转换

## Technical Decisions
| Decision | Rationale |
|----------|-----------|
| 使用 .NET 10 | 长期支持版本，性能好 |
| 先 WinForms 后 WPF | 降低学习曲线，快速出原型 |
| SQLite 本地存储 | 轻量级，无需额外部署 |
| 模块化设计 | 每个通信模块独立可测试 |
| 手动实现 Modbus | NModbus4 不兼容 .NET 10，且手动实现更利于学习理解协议 |

## Issues Encountered
| Issue | Resolution |
|-------|------------|
|       |            |

## Resources
- OPC Foundation UA .NET Standard: https://github.com/OPCFoundation/UA-.NETStandard
- OPC Foundation: https://opcfoundation.org/
- 西门子 S7 通信库: https://github.com/S7NetPlus/s7netplus
- LiveCharts2 (趋势图): https://github.com/beto-rodriguez/LiveCharts2
- TDengine (时序数据库): https://www.taosdata.com/

## Visual/Browser Findings
-

---
*Update this file after every 2 view/browser/search operations*
