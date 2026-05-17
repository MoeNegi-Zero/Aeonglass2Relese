using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Models.Encounters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

[HarmonyPatch(typeof(Glory))]
public static class AeonglassPatch
{
    public static bool? _shouldGenerateDoormakerBoss;

    private class ShouldGenerateDoormakerBossConfig
    {
        [JsonPropertyName("should_generate_doormaker_boss")]
        public bool? shouldGenerateDoormakerBoss { get; set; }
    }

    private static void LoadConfig()
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
            _shouldGenerateDoormakerBoss = false;
            return;
        }

        try
        {
            var config = JsonSerializer.Deserialize<ShouldGenerateDoormakerBossConfig>(File.ReadAllText(jsonPath));
            _shouldGenerateDoormakerBoss = config?.shouldGenerateDoormakerBoss ?? false;
        }
        catch (Exception e)
        {
            Log.Error($">>>[AeonglassMod] Failed to parse JSON: {e.Message}");
            _shouldGenerateDoormakerBoss = false;
        }
    }

    [HarmonyPatch("GenerateAllEncounters")]
    [HarmonyPostfix]
    static void Patch_AeonglassEncounter(ref IEnumerable<EncounterModel> __result)
    {
        LoadConfig();
        bool shouldGenerate = _shouldGenerateDoormakerBoss.GetValueOrDefault(false);

        var list = __result.ToList();

        if (shouldGenerate)
        {
            if (!list.Contains(ModelDb.Encounter<AeonglassBoss>()))
                list.Add(ModelDb.Encounter<AeonglassBoss>());
        }
        else
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == ModelDb.Encounter<DoormakerBoss>())
                {
                    list[i] = ModelDb.Encounter<AeonglassBoss>();
                    break;
                }
            }
        }

        __result = list;
    }

    [HarmonyPatch("get_BossDiscoveryOrder")]
    [HarmonyPostfix]
    static void Patch_AeonglassBoss(ref IEnumerable<EncounterModel> __result)
    {
        LoadConfig();
        bool shouldGenerate = _shouldGenerateDoormakerBoss.GetValueOrDefault(false);

        var list = __result.ToList();

        if (shouldGenerate)
        {
            if (!list.Contains(ModelDb.Encounter<AeonglassBoss>()))
                list.Add(ModelDb.Encounter<AeonglassBoss>());
        }
        else
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == ModelDb.Encounter<DoormakerBoss>())
                {
                    list[i] = ModelDb.Encounter<AeonglassBoss>();
                    break;
                }
            }
        }

        __result = list;
    }
}