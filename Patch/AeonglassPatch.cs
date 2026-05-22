using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Models.Powers;
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
    public static bool? _aeonglassV106;

    private class ShouldGenerateDoormakerBossConfig
    {
        [JsonPropertyName("should_generate_doormaker_boss")]
        public bool? shouldGenerateDoormakerBoss { get; set; }

        [JsonPropertyName("aeonglass_v106")]
        public bool? aeonglassV106 { get; set; }
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
            _aeonglassV106 = true;
            return;
        }

        try
        {
            var config = JsonSerializer.Deserialize<ShouldGenerateDoormakerBossConfig>(File.ReadAllText(jsonPath));
            _shouldGenerateDoormakerBoss = config?.shouldGenerateDoormakerBoss ?? false;
            _aeonglassV106 = config?.aeonglassV106 ?? true;
        }
        catch (Exception e)
        {
            Log.Error($">>>[AeonglassMod] Failed to parse JSON: {e.Message}");
            _shouldGenerateDoormakerBoss = false;
            _aeonglassV106 = true;
        }
    }

    [HarmonyPatch("GenerateAllEncounters")]
    [HarmonyPostfix]
    static void Patch_AeonglassEncounter(ref IEnumerable<EncounterModel> __result)
    {
        LoadConfig();
        bool shouldGenerate = _shouldGenerateDoormakerBoss.GetValueOrDefault(false);

        var list = __result.ToList();
        EncounterModel aeonglassModel = (bool)_aeonglassV106 ? ModelDb.Encounter<AeonglassBossV106>() : ModelDb.Encounter<AeonglassBoss>();

        if (shouldGenerate)
        {
            if (!list.Contains(aeonglassModel))
                list.Add(aeonglassModel);
        }
        else
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == ModelDb.Encounter<DoormakerBoss>())
                {
                    list[i] = aeonglassModel;
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
        EncounterModel aeonglassModel = (bool)_aeonglassV106 ? ModelDb.Encounter<AeonglassBossV106>() : ModelDb.Encounter<AeonglassBoss>();

        if (shouldGenerate)
        {
            if (!list.Contains(aeonglassModel))
                list.Add(aeonglassModel);
        }
        else
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == ModelDb.Encounter<DoormakerBoss>())
                {
                    list[i] = aeonglassModel;
                    break;
                }
            }
        }

        __result = list;
    }
}

[HarmonyPatch(typeof(ImageHelper))]
public static class AeonglassIconPatch
{
    [HarmonyPatch("GetRoomIconSuffix")]
    [HarmonyPostfix]
    static void GetRoomIconSuffixPostfix(ref string? __result)
    {
        if (__result != null)
        {
            if (__result.StartsWith("aeonglass_boss_v"))
            {
                __result = "aeonglass_boss";
            }
        }
    }

    /*
    [HarmonyPatch("GetImagePath")]
    [HarmonyPostfix]
    static void GetImagePath(ref string? __result)
    {
        if (__result != null)
        {
            if (__result.Contains("withering_presence_power_v"))
            {
                __result.Replace("withering_presence_power_v106", "withering_presence_power");
            }
        }
    }*/
}

[HarmonyPatch(typeof(PowerModel))]
public static class WitheringPresencePowerIconPatch
{
    [HarmonyPatch("get_PackedIconPath")]
    [HarmonyPostfix]
    static void PackedIconPathPostfix(PowerModel __instance, ref string __result)
    {
        if (__instance is WitheringPresencePowerV106)
        {
            __result = __result.Replace(__instance.Id.Entry.ToLowerInvariant(), "withering_presence_power");
        }
    }

    [HarmonyPatch("get_BigIconPath")]
    [HarmonyPostfix]
    static void BigIconPathPostfix(PowerModel __instance, ref string __result)
    {
        if (__instance is WitheringPresencePowerV106)
        {
            __result = __result.Replace(__instance.Id.Entry.ToLowerInvariant(), "withering_presence_power");
        }
    }

    [HarmonyPatch("get_BigBetaIconPath")]
    [HarmonyPostfix]
    static void BigBetaIconPathPostfix(PowerModel __instance, ref string __result)
    {
        if (__instance is WitheringPresencePowerV106)
        {
            __result = __result.Replace(__instance.Id.Entry.ToLowerInvariant(), "withering_presence_power");
        }
    }
}