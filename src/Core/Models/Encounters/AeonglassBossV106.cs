using Godot;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;
using System.Collections.Generic;

namespace MegaCrit.Sts2.Core.Models.Encounters;

public sealed class AeonglassBossV106 : EncounterModel
{
	public override RoomType RoomType => RoomType.Boss;

	public override MegaSkeletonDataResource? BossNodeSpineResource => null;

	public override string BossNodePath => "res://images/map/placeholder/" + "aeonglass_boss" + "_icon";

    public override string CustomBgm => "event:/music/act3_boss_queen";

	protected override bool HasCustomBackground => false;

	public override IEnumerable<MonsterModel> AllPossibleMonsters => [ModelDb.Monster<AeonglassV106>()];

	public override float GetCameraScaling()
	{
		return 0.9f;
	}

	public override Vector2 GetCameraOffset()
	{
		return Vector2.Down * 60f;
	}

	protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
	{
		return [(ModelDb.Monster<AeonglassV106>().ToMutable(), null)];
	}
}
