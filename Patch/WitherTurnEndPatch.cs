using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;

[HarmonyPatch]
public static class Patch_DoTurnEnd_WitherToHand
{
    // 指定 Patch MoveNext 方法
    static MethodBase TargetMethod()
    {
        // 获取 DoTurnEnd 的编译生成的状态机类
        var stateMachineType = typeof(CombatManager).GetNestedType("<DoTurnEnd>d__119", BindingFlags.NonPublic);
        return stateMachineType.GetMethod("MoveNext", BindingFlags.NonPublic | BindingFlags.Instance);
    }

    // Transpiler 替换 await CardPileCmd.Add(card, discardPile)
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var codes = new List<CodeInstruction>(instructions);

        // 获取原来的 Add 方法
        var addMethod = AccessTools.Method(
            typeof(CardPileCmd),
            "Add",
            new System.Type[] { typeof(CardModel), typeof(CardPile), typeof(CardPilePosition), typeof(MegaCrit.Sts2.Core.Models.AbstractModel), typeof(bool) }
        );

        // 获取我们自定义的 async Add 方法
        var addWitherMethod = AccessTools.Method(
            typeof(Patch_DoTurnEnd_WitherToHand),
            nameof(AddWitherToHandAsync)
        );

        for (int i = 0; i < codes.Count; i++)
        {
            if (codes[i].opcode == OpCodes.Call && codes[i].operand == addMethod)
            {
                // 替换调用
                codes[i].operand = addWitherMethod;
            }
        }

        return codes.AsEnumerable();
    }

    // async 方法处理 Wither 回手牌
    public static async Task<CardPileAddResult> AddWitherToHandAsync(CardModel card, CardPile discardPile, CardPilePosition position = CardPilePosition.Bottom, MegaCrit.Sts2.Core.Models.AbstractModel source = null, bool skipVisuals = false)
    {
        if (card is Wither)
        {
            // Wither 回手牌
            return await CardPileCmd.Add(card, PileType.Hand, position, source, skipVisuals);
        }
        else
        {
            // 其他卡牌正常弃牌
            return await CardPileCmd.Add(card, discardPile, position, source, skipVisuals);
        }
    }
}