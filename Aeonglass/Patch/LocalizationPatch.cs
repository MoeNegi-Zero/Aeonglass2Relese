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
			["WITHER.title"] = "凋萎",
            ["WITHER_V106.description"] = "在你的回合结束时，如果这张牌在你的[gold]手牌[/gold]中，你受到{Damage:diff()}点伤害。",
            ["WITHER_V106.title"] = "凋萎"
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
            ["WITHERING_PRESENCE_POWER.title"] = "凋萎存在",
            ["WITHERING_PRESENCE_POWER_V106.description"] = "你每打出[blue]6[/blue]张牌，将一张[gold]凋萎[/gold]加入你的[gold]手牌[/gold]，并将所有[gold]凋零[/gold]牌的伤害增加[blue]3[/blue]。",
            ["WITHERING_PRESENCE_POWER_V106.smartDescription"] = "你每打出[blue]6[/blue]张牌，将一张[gold]凋萎[/gold]加入你的[gold]手牌[/gold]，并将所有[gold]凋零[/gold]牌的伤害增加[blue]3[/blue]。",
            ["WITHERING_PRESENCE_POWER_V106.title"] = "凋萎存在",
        });

		var monstersTable = LocManager.Instance.GetTable("monsters");

		// 中文
		monstersTable.MergeWith(new Dictionary<string, string>
		{
            ["AEONGLASS.moves.EBB.title"]= "消退",
            ["AEONGLASS.moves.EYE_LASERS.title"]= "眼部激光",
            ["AEONGLASS.moves.INCREASING_INTENSITY.title"]= "加大力度",
            ["AEONGLASS.name"]= "永世沙漏",
            ["AEONGLASS_V106.moves.EBB.title"] = "消退",
            ["AEONGLASS_V106.moves.EYE_LASERS.title"] = "眼部激光",
            ["AEONGLASS_V106.moves.INCREASING_INTENSITY.title"] = "加大力度",
            ["AEONGLASS_V106.name"] = "永世沙漏"
        });

		var encountersTable = LocManager.Instance.GetTable("encounters");

		encountersTable.MergeWith(new Dictionary<string, string>
		{
            ["AEONGLASS_BOSS.loss"]= "[gold]{encounter}[/gold]受够了{character}的侵扰。",
			["AEONGLASS_BOSS.title"]= "永世沙漏",
            ["AEONGLASS_BOSS_V106.loss"] = "[gold]{encounter}[/gold]受够了{character}的侵扰。",
            ["AEONGLASS_BOSS_V106.title"] = "永世沙漏"
        });

        var relicsTable = LocManager.Instance.GetTable("relics");

        relicsTable.MergeWith(new Dictionary<string, string>
        {
            ["FISHING_ROD.description"] = "每[blue]{Combats}[/blue]场普通战斗，随机[gold]升级[/gold]你[gold]牌组[/gold]中的一张牌。",
            ["FISHING_ROD.flavor"] = "[red]这个遗物的细节将在未来揭晓……[/red]",
            ["FISHING_ROD.title"] = "钓鱼竿",
            ["KALEIDOSCOPE.description"] = "拾起时，获得[blue]{Cards}[/blue]次来自其他角色的卡牌奖励。",
            ["KALEIDOSCOPE.eventDescription"] = "获得[blue]{Cards}[/blue]次来自其他角色的卡牌奖励。",
            ["KALEIDOSCOPE.flavor"] = "[red]这个遗物的细节将在未来揭晓……[/red]",
            ["KALEIDOSCOPE.title"] = "万花筒",
            ["SILKEN_TRESS.description"] = (bool)ConfigFile._newRelicsV106 ? "拾起时，失去所有[gold]金币[/gold]。为第一次卡牌奖励中的所有牌[gold]附魔[/gold]：[purple]华彩[/purple]。" : "为第一次卡牌奖励中的所有牌[gold]附魔[/gold]：[purple]华彩[/purple]。",
            ["SILKEN_TRESS.eventDescription"] = (bool)ConfigFile._newRelicsV106 ? "失去所有[gold]金币[/gold]。为第一次卡牌奖励中的所有牌[gold]附魔[/gold]：[purple]华彩[/purple]。" : "为第一次卡牌奖励中的所有牌[gold]附魔[/gold]：[purple]华彩[/purple]。",
            ["SILKEN_TRESS.flavor"] = "[red]这个遗物的细节将在未来揭晓……[/red]",
            ["SILKEN_TRESS.title"] = "华美发束"
        });
    }
}
