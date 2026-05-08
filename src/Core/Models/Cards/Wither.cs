using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Wither : CardModel
{
	public override int MaxUpgradeLevel => 0;

	protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(2m, ValueProp.Unpowered | ValueProp.Move)];

	public override IEnumerable<CardKeyword> CanonicalKeywords => [
		CardKeyword.Exhaust,
		CardKeyword.Retain
	];

	public override bool HasTurnEndInHandEffect => true;

	protected override IEnumerable<string> ExtraRunAssetPaths => NGroundFireVfx.AssetPaths;

	public Wither()
		: base(1, CardType.Status, CardRarity.Status, TargetType.None)
	{
	}

	public override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
	{
		await CreatureCmd.Damage(choiceContext, Owner.Creature, DynamicVars.Damage, this);
	}
	/*
	protected override PileType GetResultPileTypeForOnTurnEndInHandEffect()
	{
		return PileType.Hand;
	}
	*/
}
