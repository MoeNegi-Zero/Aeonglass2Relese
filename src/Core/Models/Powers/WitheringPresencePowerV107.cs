using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Monsters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class WitheringPresencePowerV107 : PowerModel
{
    private const int _baseCardsLeft = 6;

	private const string _cardsLeftKey = "CardsLeft";

	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Counter;

	public override int DisplayAmount => DynamicVars["CardsLeft"].IntValue;

    public override bool IsInstanced => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("CardsLeft", 6m)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            WitherV106 wither = (WitherV106)ModelDb.Card<WitherV106>().ToMutable();
            if (base.Owner.Monster is AeonglassV107 aeonglass)
            {
                aeonglass.MatchWitherToUpgradeCount(wither);
            }
            List<IHoverTip> list = new List<IHoverTip>();
            list.Add(HoverTipFactory.FromCard(wither));
            list.AddRange(ModelDb.Card<WitherV106>().HoverTips);
            return list;
        }
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if (cardPlay.Card.Owner == Target.Player)
		{
			DynamicVars["CardsLeft"].BaseValue--;
			InvokeDisplayAmountChanged();
			if (DynamicVars["CardsLeft"].IntValue <= 0)
			{
				await Cmd.Wait(0.5f);
				await CardPileCmd.AddToCombatAndPreview<WitherV106>([cardPlay.Card.Owner.Creature], PileType.Hand, 1, false);
				Flash();
				DynamicVars["CardsLeft"].BaseValue = 6m;
				InvokeDisplayAmountChanged();
			}
		}
	}
}
