using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.ValueProps;

namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class Aeonglass : MonsterModel
{
	private const string _doormakerTrackName = "queen_progress";

	private int _additionalStrength;

	public override int MinInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 535, 512);

	public override int MaxInitialHp => MinInitialHp;

	public override DamageSfxType TakeDamageSfxType => DamageSfxType.Stone;

	private int EbbDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 32, 26);

	private int EyeLasersDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 12, 11);

	private int EyeLasersRepeat => 2;

	private int IncreasingIntensityBlock => 33;

	private int IncreasingIntensityBaseStrength => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);

	private int AdditionalStrength
	{
		get
		{
			return _additionalStrength;
		}
		set
		{
			AssertMutable();
			_additionalStrength = value;
		}
	}

	private int IncreasingIntensityTotalStrength => IncreasingIntensityBaseStrength + AdditionalStrength;

	public override async Task AfterAddedToRoom()
	{
		await base.AfterAddedToRoom();
		foreach (Player player in Creature.CombatState.Players)
		{
			WitheringPresencePower witheringPresencePower = (WitheringPresencePower)ModelDb.Power<WitheringPresencePower>().ToMutable();
			witheringPresencePower.Target = player.Creature;
			//await PowerCmd.Apply(new ThrowingPlayerChoiceContext(), witheringPresencePower, base.Creature, 4m, base.Creature, null);

			await PowerCmd.Apply(witheringPresencePower, Creature, 4m, Creature, null);
        }
		//await PowerCmd.Apply<ArtifactPower>(new ThrowingPlayerChoiceContext(), base.Creature, 3m, base.Creature, null);
        await PowerCmd.Apply<ArtifactPower>(Creature, 3m, Creature, null);
    }

	public override Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
	{
		if (creature != Creature)
		{
			return Task.CompletedTask;
		}
		NRunMusicController.Instance?.UpdateMusicParameter("queen_progress", 5f);
		return Task.CompletedTask;
	}

	protected override MonsterMoveStateMachine GenerateMoveStateMachine()
	{
		List<MonsterState> list = new List<MonsterState>();
		MoveState moveState = new MoveState("EBB_MOVE", EbbMove, new SingleAttackIntent(EbbDamage), new DebuffIntent());
		MoveState moveState2 = new MoveState("EYE_LASERS_MOVE", EyeLasersMove, new MultiAttackIntent(EyeLasersDamage, EyeLasersRepeat));
		MoveState moveState3 = new MoveState("INCREASING_INTENSITY_MOVE", IncreasingIntensityMove, new BuffIntent(), new DefendIntent());
		moveState.FollowUpState = moveState2;
		moveState2.FollowUpState = moveState3;
		moveState3.FollowUpState = moveState;
		list.Add(moveState);
		list.Add(moveState2);
		list.Add(moveState3);
		return new MonsterMoveStateMachine(list, moveState);
	}

	private async Task EbbMove(IReadOnlyList<Creature> targets)
	{
		await DamageCmd.Attack(EbbDamage).FromMonster(this).WithAttackerAnim("Attack", 0.15f)
			.WithHitFx("vfx/vfx_attack_blunt")
			.Execute(null);
        //await PowerCmd.Apply<EbbPower>(new ThrowingPlayerChoiceContext(), targets, 3m, base.Creature, null);
        await PowerCmd.Apply<EbbPower>(targets, 3m, Creature, null);
    }

	private async Task EyeLasersMove(IReadOnlyList<Creature> targets)
	{
		await DamageCmd.Attack(EyeLasersDamage).FromMonster(this).WithHitCount(EyeLasersRepeat)
			.WithAttackerAnim("Attack", 0.15f)
			.WithHitFx("vfx/vfx_attack_blunt")
			.Execute(null);
	}

	private async Task IncreasingIntensityMove(IReadOnlyList<Creature> targets)
	{
        //await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Creature, IncreasingIntensityTotalStrength, base.Creature, null);
        await PowerCmd.Apply<StrengthPower>(Creature, IncreasingIntensityTotalStrength, Creature, null);
        AdditionalStrength++;
		await CreatureCmd.GainBlock(Creature, IncreasingIntensityBlock, ValueProp.Move, null);
	}
}
