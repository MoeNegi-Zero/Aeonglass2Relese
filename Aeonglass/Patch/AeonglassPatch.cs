using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Models.Cards;
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
	[HarmonyPatch("GenerateAllEncounters")]
	[HarmonyPostfix]
	static void Patch_AeonglassEncounter(ref IEnumerable<EncounterModel> __result)
	{
		bool shouldGenerate = ConfigFile._shouldGenerateDoormakerBoss.GetValueOrDefault(false);

		var list = __result.ToList();
		EncounterModel aeonglassModel = (bool)ConfigFile._aeonglassV107 ? ModelDb.Encounter<AeonglassBossV107>() : ModelDb.Encounter<AeonglassBoss>();

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
		bool shouldGenerate = ConfigFile._shouldGenerateDoormakerBoss.GetValueOrDefault(false);

		var list = __result.ToList();
		EncounterModel aeonglassModel = (bool)ConfigFile._aeonglassV107 ? ModelDb.Encounter<AeonglassBossV107>() : ModelDb.Encounter<AeonglassBoss>();

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
}

[HarmonyPatch(typeof(PowerModel))]
public static class WitheringPresencePowerIconPatch
{
	[HarmonyPatch("get_PackedIconPath")]
	[HarmonyPostfix]
	static void PackedIconPathPostfix(PowerModel __instance, ref string __result)
	{
		if (__instance is WitheringPresencePowerV106 || __instance is WitheringPresencePowerV107)
		{
			__result = __result.Replace(__instance.Id.Entry.ToLowerInvariant(), "withering_presence_power");
		}
	}

	[HarmonyPatch("get_BigIconPath")]
	[HarmonyPostfix]
	static void BigIconPathPostfix(PowerModel __instance, ref string __result)
	{
		if (__instance is WitheringPresencePowerV106 || __instance is WitheringPresencePowerV107)
		{
			__result = __result.Replace(__instance.Id.Entry.ToLowerInvariant(), "withering_presence_power");
		}
	}

	[HarmonyPatch("get_BigBetaIconPath")]
	[HarmonyPostfix]
	static void BigBetaIconPathPostfix(PowerModel __instance, ref string __result)
	{
		if (__instance is WitheringPresencePowerV106 || __instance is WitheringPresencePowerV107)
		{
			__result = __result.Replace(__instance.Id.Entry.ToLowerInvariant(), "withering_presence_power");
		}
	}
}

[HarmonyPatch(typeof(CardModel))]
public static class WitherV106TitlePatch
{
	[HarmonyPatch("get_Title")]
	[HarmonyPostfix]
	static void WitherV106TitlePostfix(CardModel __instance, ref string __result)
	{
		if (__instance is WitherV106 card)
		{
			if (card.FakeUpgradeLevel > 0)
			{
				__result= $"{__result}+{card.FakeUpgradeLevel}";
			}
		}
	}
}
