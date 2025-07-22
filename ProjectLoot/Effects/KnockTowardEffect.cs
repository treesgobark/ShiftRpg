using Microsoft.Xna.Framework;
using ProjectLoot.Effects.Base;

namespace ProjectLoot.Effects;

public class KnockTowardEffect : IEffect
{
    public Team AppliesTo { get; init; }
    public SourceTag Source { get; init; }
    public TimeSpan Duration { get; init; }
    public Vector3 TargetPosition { get; init; }
}