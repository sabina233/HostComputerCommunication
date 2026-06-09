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
/// 简单日志记录器
/// 通过事件机制将日志消息传递给 UI 层显示
/// </summary>
public class Logger
{
    /// <summary>日志事件，UI 控件订阅此事件来显示日志</summary>
    public event EventHandler<LogEventArgs>? LogReceived;

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
        LogReceived?.Invoke(this, new LogEventArgs(level, message, source));
    }
}
