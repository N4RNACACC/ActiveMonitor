namespace ActiveMonitor;

public class AppInfo
{
    public static readonly string AppName = "ActiveMonitor";
    public static readonly string AppPath = AppContext.BaseDirectory;
    public static readonly string AppDataPath = Path.Combine(AppPath, "ActiveMonitor");
    public static readonly string ConfigPath = Path.Combine(AppDataPath, "configs");
    public static readonly string LogPath = Path.Combine(AppDataPath, "logs");
}
