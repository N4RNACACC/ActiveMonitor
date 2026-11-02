// Top-level statements enabled
// encoding: UTF-8
using System.Diagnostics;
using ActiveMonitor;

Console.Title = AppInfo.AppName;

Logger.Initialize();
Logger.LogInfo("ActiveMonitor is starting...");

var targetProcess = new Process();
var targetProcessPath = Path.Combine(AppInfo.AppPath, "Autovisor.exe");
targetProcess.StartInfo.FileName = targetProcessPath;
targetProcess.StartInfo.UseShellExecute = true;
if(!File.Exists(targetProcessPath))
{
    Logger.LogError($"Target process not found: {targetProcessPath}");
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
}
else
{
    Logger.LogInfo($"Monitoring target process: {targetProcessPath}");
    if(Process.GetProcessesByName("Autovisor").Length > 0)
    {
        Logger.LogInfo("Target process is already running.");
        Logger.LogInfo($"Monitoring target process PID: {Process.GetProcessesByName("Autovisor")[0].Id}");
    }
    else
    {
        Logger.LogWarning("Target process is not running. Starting...");
        targetProcess.Start();
    }
    while (true)
    {
         if(targetProcess.HasExited)
         {
             Logger.LogWarning("Target process not active. Restarting...");
             try
             {
                 targetProcess.Start();
                 Logger.LogInfo("Target process restarted successfully.");
                 Logger.LogInfo($"Monitoring target process PID: {targetProcess.Id}");
             }
             catch (Exception ex)
             {
                 Logger.LogError($"Failed to restart target process: {ex.Message}");
                 Console.WriteLine("Press any key to exit...");
                 Console.ReadKey();
                 break;
             }
         }
         targetProcess.WaitForExit();
    }
}   
