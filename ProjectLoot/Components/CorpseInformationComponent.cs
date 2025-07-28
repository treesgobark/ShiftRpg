using FlatRedBall.Graphics.Animation;
using ProjectLoot.Components.Interfaces;

namespace ProjectLoot.Components;

public class CorpseInformationComponent : ICorpseInformationComponent
{
    public required AnimationChainList BodyAnimationChains { get; set; }
    public required string BodyChainName { get; set; }
    public required AnimationChainList ExplosionAnimationChains { get; set; }
    public required string ExplosionChainName { get; set; }
    public TimeSpan HitstopDuration { get; set; }
}