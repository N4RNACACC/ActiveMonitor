namespace ActiveMonitor;

public class Logger
{
    private enum LogLevel
    {
        Info,
        Warn,
        Error
    }
    private static readonly string LogDirPath = Path.Combine(AppInfo.AppPath, "ActiveMonitor", "logs");
    private static bool _lock = true; // define limit locker
    private static string _logFilePath = "";

    public static void Initialize()
    {
        Console.WriteLine("Initializing Logger...");
        if (!Directory.Exists(LogDirPath))
        {
            try
            {
                Directory.CreateDirectory(LogDirPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to create log directory: {ex.Message}");
                Console.WriteLine("Logger will not be initialized.");
                _lock = true;
                return;
            }

            try
            {
                var count = 1;
                while (true)
                {
                    var logFileName = $"log_{DateTime.Now:yyyyMMdd}_{count}.log";
                    _logFilePath = Path.Combine(LogDirPath, logFileName);
                    if (!File.Exists(_logFilePath))
                    {
                        File.Create(_logFilePath).Dispose();
                        break;
                    }
                    count++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to create log file: {ex.Message}");
                Console.WriteLine("Logger will not be initialized.");
                _lock = true;
                return;
            }
            _lock = false; // unlock limit locker
        }

        Console.WriteLine(_lock
            ? "Logger is locked. Logging will not be available."
            : $"Logger initialized. Log file: {_logFilePath}");
    }

    private static void WriteToFile(string content)
    {
        try
        {
            if (_lock) return;
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
        if (_lock)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(content);
            return;
        }
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(content);
        WriteToFile(content);
    }


    public static void LogWarning(string message)
    {
        var content = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}][{LogLevel.Warn}]: {message}";
        if (_lock)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(content);
            return;
        }
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(content);
        WriteToFile(content);
    }

    public static void LogError(string message)
    {
        var content = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}][{LogLevel.Error}]: {message}";
        if (_lock)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(content);
            return;
        }
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(content);
        WriteToFile(content);
    }

}
