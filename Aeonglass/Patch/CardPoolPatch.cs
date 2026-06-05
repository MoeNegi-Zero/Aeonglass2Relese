using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using System.Linq;

[HarmonyPatch(typeof(StatusCardPool), "GenerateAllCards")]
public static class CardPoolPatch
{
	static void Postfix(ref CardModel[] __result)
	{
		var cards = __result.ToList();
		cards.Add((bool)ConfigFile._aeonglassV107? ModelDb.Card<WitherV106>():ModelDb.Card<Wither>());
		__result = cards.ToArray();
	}
}
