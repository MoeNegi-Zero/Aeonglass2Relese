using HarmonyLib;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Relics;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

[HarmonyPatch(typeof(EventRelicPool), "GenerateAllRelics")]
public static class RelicPoolPatch
{
    static void Postfix(ref IEnumerable<RelicModel> __result)
    {
        var relics = __result.ToList();
        relics.Add(ModelDb.Relic<FishingRod>());
        relics.Add(ModelDb.Relic<Kaleidoscope>());
        relics.Add(ModelDb.Relic<SilkenTress>());
        __result = relics;
    }
}

[HarmonyPatch(typeof(Neow), "get_PositiveOptions")]
public static class NeowOptionPatch
{
    static void Postfix(Neow __instance, ref IEnumerable<EventOption> __result)
    {
        var relics = __result.ToList();
        MethodInfo relicOptionMethod = AccessTools.Method(
            typeof(AncientEventModel),
            "RelicOption",
            new System.Type[]
            {
                typeof(RelicModel),
                typeof(string),
                typeof(string)
            });

        EventOption fishingRod = (EventOption)relicOptionMethod.Invoke(
            __instance,
            new object[]
            {
                ModelDb.Relic<FishingRod>().ToMutable(),
                "INITIAL",
                "NEOW.pages.DONE.POSITIVE.description"
            });

        EventOption kaleidoscope = (EventOption)relicOptionMethod.Invoke(
            __instance,
            new object[]
            {
                ModelDb.Relic<Kaleidoscope>().ToMutable(),
                "INITIAL",
                "NEOW.pages.DONE.POSITIVE.description"
            });

        EventOption silkenTress = (EventOption)relicOptionMethod.Invoke(
            __instance,
            new object[]
            {
                ModelDb.Relic<SilkenTress>().ToMutable(),
                "INITIAL",
                "NEOW.pages.DONE.POSITIVE.description"
            });
        
        relics.Add(fishingRod);
        relics.Add(kaleidoscope);
        relics.Add(silkenTress);

        __result = relics.ToArray();
    }
}