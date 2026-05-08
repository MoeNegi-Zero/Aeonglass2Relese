using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[HarmonyPatch(typeof(Glory), "GenerateAllEncounters")]
public static class Patch_AeonglassEncounter
{
    static void Postfix(ref IEnumerable<EncounterModel> __result)
    {
        var list = __result.ToList();
        list.Add(ModelDb.Encounter<AeonglassBoss>());
        __result = list;
    }
}

[HarmonyPatch(typeof(Glory), "get_BossDiscoveryOrder")]
public static class Patch_AeonglassBoss
{
    static void Postfix(ref IEnumerable<EncounterModel> __result)
    {
        var list = __result.ToList();
        list.Add(ModelDb.Encounter<AeonglassBoss>());
        __result = list;
    }
}

