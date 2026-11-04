// Top-level statements enabled
// encoding: UTF-8
using System.Diagnostics;
using System.Reflection;
using ActiveMonitor;
using ActiveMonitor.Services;

Console.Title = AppInfo.AppName;

Logger.Initialize();
Logger.LogInfo("ActiveMonitor is starting...");

// 初始化配置
var ymlPath = Path.Combine(AppInfo.ConfigPath, "config.yml");
if(!File.Exists(ymlPath))
{
    // 创建默认配置文件
    const string resourceName = "ActiveMonitor.Config.config.yml";
    const string targetFileName = "config.yml";

    // 创建配置目录
    Logger.LogInfo("Creating default configuration...");
    Directory.CreateDirectory(AppInfo.ConfigPath);

    // 将嵌入资源写入配置文件
    var assembly = Assembly.GetExecutingAssembly();
    using var stream = assembly.GetManifestResourceStream(resourceName);
    var targetPath = Path.Combine(AppInfo.AppDataPath, targetFileName);
    var tempPath = targetPath + ".tmp";
    using var resourceStream = assembly.GetManifestResourceStream(resourceName);
    if (resourceStream == null)
    {
        Logger.LogError($"Failed to load embedded resource: {resourceName}");
        Logger.LogError("Please ensure the resource is correctly embedded in the assembly.");
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
        Environment.Exit(-1);
    }
    using (var fileStream = new FileStream(tempPath,
                                            FileMode.Create,
                                            FileAccess.Write,
                                            FileShare.None))
    {
        resourceStream.CopyTo(fileStream);
    }
    File.Move(tempPath, ymlPath);
    Logger.LogInfo($"Default configuration created at: {ymlPath}");
    Logger.LogInfo("Please edit the configuration file and restart the application.");
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
    Environment.Exit(1);
}

var config = new ConfigService().Load(ymlPath);
Logger.LogInfo("Configuration loaded successfully.");
Logger.LogInfo("Checking configuration...");

// 执行配置检查
if(config.Monitor.Target.Process == null || config.Monitor.Target.Path == null)
{
    Logger.LogError("Target process or path is not specified in the configuration.");
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
    Environment.Exit(1);
}

// 构建目标进程配置
var targetProcess = new Process();
var targetProcessPath = config.Monitor.Target.Path;
targetProcess.StartInfo.FileName = targetProcessPath;
targetProcess.StartInfo.UseShellExecute = true;

// 检查目标进程是否存在
if(!File.Exists(targetProcessPath))
{
    Logger.LogError($"Target process not found: {targetProcessPath}");
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
    Environment.Exit(2);
}
else
{
    // 检查目标进程是否已经在运行
    Logger.LogInfo($"Monitoring target process: {targetProcessPath}");
    if(Process.GetProcessesByName(config.Monitor.Target.Process).Length > 0)
    {
        Logger.LogInfo("Target process is already running.");
        Logger.LogInfo($"Monitoring target process [PID: {Process.GetProcessesByName(config.Monitor.Target.Process)[0].Id}]");
    }
    else
    {
        Logger.LogWarning("Target process is not running. Starting...");
        targetProcess.Start();
    }
    // 开始监控loop
    while (true)
    {
         if(targetProcess.HasExited)
         {
             Logger.LogWarning("Target process not active. Restarting...");
             try
             {
                 targetProcess.Start();
                 Logger.LogInfo("Target process restarted successfully.");
                 Logger.LogInfo($"Monitoring target process [PID: {targetProcess.Id}]");
             }
             catch (Exception ex)
             {
                 Logger.LogError($"Failed to restart target process: {ex.Message}");
                 Console.WriteLine("Press any key to exit...");
                 Console.ReadKey();
                 Environment.Exit(-1);
             }
         }
         targetProcess.WaitForExit();
    }
}   
