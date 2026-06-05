namespace HostComputerCommunication.Common.Helpers;

/// <summary>
/// 日志级别
/// </summary>
public enum LogLevel
{
    Debug,
    Info,
    Warning,
    Error
}

/// <summary>
/// 日志事件参数
/// </summary>
public class LogEventArgs : EventArgs
{
    public DateTime Timestamp { get; }
    public LogLevel Level { get; }
    public string Message { get; }
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
/// </summary>
public class Logger
{
    public event EventHandler<LogEventArgs>? LogReceived;

    public void Debug(string message, string? source = null)
        => Log(LogLevel.Debug, message, source);

    public void Info(string message, string? source = null)
        => Log(LogLevel.Info, message, source);

    public void Warning(string message, string? source = null)
        => Log(LogLevel.Warning, message, source);

    public void Error(string message, string? source = null)
        => Log(LogLevel.Error, message, source);

    private void Log(LogLevel level, string message, string? source)
    {
        LogReceived?.Invoke(this, new LogEventArgs(level, message, source));
    }
}
