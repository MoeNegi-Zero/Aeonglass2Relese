using Godot;
using MegaCrit.Sts2.Core.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

public static class ConfigFile
{
    public static bool? _shouldGenerateDoormakerBoss;
    public static bool? _aeonglassV106;
    public static bool? _shouldActivateNewRelics;
    public static bool? _newRelicsV106;

    private class Config
    {
        [JsonPropertyName("should_generate_doormaker_boss")]
        public bool? shouldGenerateDoormakerBoss { get; set; }

        [JsonPropertyName("aeonglass_v106")]
        public bool? aeonglassV106 { get; set; }

        [JsonPropertyName("should_activate_new_relics")]
        public bool? shouldActivateNewRelics { get; set; }

        [JsonPropertyName("new_relics_v106")]
        public bool? newRelicsV106 { get; set; }
    }

    public static void LoadConfig()
    {
        string exeDir = Path.GetDirectoryName(OS.GetExecutablePath());
        string[] paths =
        {
            Path.Combine(exeDir, "Mods", "Aeonglass", "AeonglassConfig.json"),
            Path.Combine(exeDir, "mods", "Aeonglass", "AeonglassConfig.json"),
            Path.Combine(exeDir, "Mods", "AeonglassConfig.json"),
            Path.Combine(exeDir, "mods", "AeonglassConfig.json")
        };

        string? jsonPath = paths.FirstOrDefault(File.Exists);

        if (jsonPath == null)
        {
            Log.Warn(">>>[AeonglassMod]You monster!!! Where do you hide the json file?");
            LoadDefalutConfig();
            return;
        }

        try
        {
            var config = JsonSerializer.Deserialize<Config>(File.ReadAllText(jsonPath));
            _shouldGenerateDoormakerBoss = config?.shouldGenerateDoormakerBoss ?? false;
            _aeonglassV106 = config?.aeonglassV106 ?? true;
            _shouldActivateNewRelics = config?.shouldActivateNewRelics ?? true;
            _newRelicsV106 = config?.newRelicsV106 ?? true;
        }
        catch (Exception e)
        {
            Log.Error($">>>[AeonglassMod] Failed to parse JSON: {e.Message}");
            LoadDefalutConfig();
        }
    }

    private static void LoadDefalutConfig()
    {
        _shouldGenerateDoormakerBoss = false;
        _aeonglassV106 = true;
        _shouldActivateNewRelics = true;
        _newRelicsV106 = true;
    }
}