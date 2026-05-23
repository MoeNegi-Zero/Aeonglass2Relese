using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class WitherV106 : CardModel
{
    public override string PortraitPath => ImageHelper.GetImagePath($"atlases/card_atlas.sprites/{Pool.Title.ToLowerInvariant()}/wither.tres");

    private int _fakeUpgradeLevel;

    public int FakeUpgradeLevel
    {
        get
        {
            return _fakeUpgradeLevel;
        }
        set
        {
            AssertMutable();
            _fakeUpgradeLevel = value;
        }
    }

    public override int MaxUpgradeLevel => 0;

	protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(3m, ValueProp.Unpowered | ValueProp.Move)];

	public override IEnumerable<CardKeyword> CanonicalKeywords => [
		CardKeyword.Unplayable
	];

	public override bool HasTurnEndInHandEffect => true;

	protected override IEnumerable<string> ExtraRunAssetPaths => NGroundFireVfx.AssetPaths;

	public WitherV106()
		: base(-1, CardType.Status, CardRarity.Status, TargetType.None)
	{
	}

    public void FakeUpgrade()
    {
        FakeUpgradeLevel++;
        DynamicVars.Damage.UpgradeValueBy(3m);
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
