using Microsoft.Xna.Framework;
using ProjectLoot.Effects.Base;

namespace ProjectLoot.Effects;

public class KnockTowardEffect : IEffect
{
    public required Team AppliesTo { get; init; }
    public required SourceTag Source { get; init; }
    public required Vector3 TargetPosition { get; init; }
    public float Strength { get; init; } = 400f;
}