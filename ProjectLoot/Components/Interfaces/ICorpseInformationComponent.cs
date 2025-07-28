using FlatRedBall.Graphics.Animation;

namespace ProjectLoot.Components.Interfaces;

public interface ICorpseInformationComponent
{
    AnimationChainList BodyAnimationChains { get; }
    string BodyChainName { get; }
    AnimationChainList ExplosionAnimationChains { get; }
    string ExplosionChainName { get; }
    TimeSpan HitstopDuration { get; set; }
}