
using ActiveMonitor.Config;
using YamlDotNet.Serialization;

namespace ActiveMonitor.Services;

public class ConfigService
{
    private readonly IDeserializer _deserializer;

    public ConfigService()
    {
        var context = new ConfigStaticContext();
        _deserializer = new StaticDeserializerBuilder(context).Build();
    }

    public RootConfig Load(string yamlPath)
    {
        var yaml = File.ReadAllText(yamlPath);
        return _deserializer.Deserialize<RootConfig>(yaml);
    }
}
