using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Localization;
using System.Collections.Generic;

[HarmonyPatch(typeof(ModelDb), "Init")]
public static class LocalizationPatch
{
	static void Postfix()
	{
		// 确保 cards 表已加载
		var cardsTable = LocManager.Instance.GetTable("cards");

		// 中文
		cardsTable.MergeWith(new Dictionary<string, string>
		{
			["WITHER.description"] = "在你的回合结束时，如果这张牌在你的[gold]手牌[/gold]中，你受到{Damage:diff()}点伤害。",
			["WITHER.title"] = "凋萎"
        });

		var powersTable = LocManager.Instance.GetTable("powers");

		// 中文
		powersTable.MergeWith(new Dictionary<string, string>
		{
			["EBB_POWER.description"] = "在本回合失去[blue]3[/blue]点[gold]力量[/gold]和[blue]3[/blue]点[gold]敏捷[/gold]。",
			["EBB_POWER.smartDescription"] = "在本回合失去[blue]{Amount}[/blue]点[gold]力量[/gold]和[blue]{Amount}[/blue]点[gold]敏捷[/gold]。",
			["EBB_POWER.title"] = "消退",
            ["WITHERING_PRESENCE_POWER.description"] = "你每打出[blue]4[/blue]张非状态牌，将一张[gold]凋萎[/gold]加入你的[gold]手牌[/gold]。",
            ["WITHERING_PRESENCE_POWER.smartDescription"] = "你每打出[blue]4[/blue]张非状态牌，将一张[gold]凋萎[/gold]加入你的[gold]手牌[/gold]。",
            ["WITHERING_PRESENCE_POWER.title"] = "凋萎存在"
        });

		var monstersTable = LocManager.Instance.GetTable("monsters");

		// 中文
		monstersTable.MergeWith(new Dictionary<string, string>
		{
            ["AEONGLASS.moves.EBB.title"]= "消退",
            ["AEONGLASS.moves.EYE_LASERS.title"]= "眼部激光",
            ["AEONGLASS.moves.INCREASING_INTENSITY.title"]= "加大力度",
            ["AEONGLASS.name"]= "永世沙漏"
        });
    }
}
