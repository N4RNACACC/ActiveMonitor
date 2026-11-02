using YamlDotNet.Serialization;

namespace ActiveMonitor.Config;

[YamlSerializable]
public class RootConfig
{
    public MonitorConfig Monitor { get; set; } = new();
    public LoggerConfig Logger { get; set; } = new();
}

[YamlSerializable]
public class MonitorConfig
{
    public MonitorTargetConfig Target { get; set; } = new();
    public MonitorRetryConfig Retry { get; set; } = new();
}

[YamlSerializable]
public class MonitorTargetConfig
{
    public string? Process { get; set; }
    public string? Path { get; set; }
}

[YamlSerializable]
public class MonitorRetryConfig
{
    public int Delay { get; set; } = 0;
}

[YamlSerializable]
public class LoggerConfig
{
    public bool Enabled { get; set; } = false;
}

[YamlStaticContext]
public partial class ConfigStaticContext;