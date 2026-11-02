namespace ActiveMonitor;

public class Logger
{
    private enum LogLevel
    {
        Info,
        Warn,
        Error
    }
    private static bool _writeLock = true; // define write locker
    private static string _logFilePath = string.Empty;
    
    public static void Initialize()
    {
        Console.WriteLine("Initializing Logger...");
        // 检查文件目录
        if (!Directory.Exists(AppInfo.LogPath))
        {
            try
            {
                Directory.CreateDirectory(AppInfo.LogPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to create log directory: {ex.Message}");
                Console.WriteLine("Logger will not be initialized.");
                _writeLock = true;
                return;
            }
        }
        // 创建日志文件
        try
        {
            var count = 1;
            while (true)
            {
                var logFileName = $"log_{DateTime.Now:yyyyMMdd}_{count}.log";
                _logFilePath = Path.Combine(AppInfo.LogPath, logFileName);
                if (!File.Exists(_logFilePath))
                {
                    File.Create(_logFilePath).Dispose();
                    _writeLock = false; // unlock write locker
                    break;
                }
                count++;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to create log file: {ex.Message}");
            Console.WriteLine("Logger will not be fully initialized.");
            _writeLock = true;
            return;
        }
        Console.WriteLine(_writeLock
            ? "Logger is locked. Logging will not be available."
            : $"Logger initialized. Log file: {_logFilePath}");
    }

    private static void WriteToFile(string content)
    {
        try
        {
            if (_writeLock) return;
            File.AppendAllText(_logFilePath, content + Environment.NewLine);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to write to log file: {ex.Message}");
        }
    }

    public static void LogInfo(string message)
    {
        var content = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}][{LogLevel.Info}]: {message}";
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(content);
        if (_writeLock)
        {
            return;
        }
        WriteToFile(content);
    }


    public static void LogWarning(string message)
    {
        var content = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}][{LogLevel.Warn}]: {message}";
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(content);
        if (_writeLock)
        {
            return;
        }
        WriteToFile(content);
    }

    public static void LogError(string message)
    {
        var content = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}][{LogLevel.Error}]: {message}";
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(content);
        if (_writeLock)
        {
            return;
        }
        WriteToFile(content);
    }

}
