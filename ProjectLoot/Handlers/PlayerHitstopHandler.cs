using ANLG.Utilities.Core.NonStaticUtilities;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.Handlers.Base;

namespace ProjectLoot.Effects.Handlers;

public class PlayerHitstopHandler : EffectHandler<HitstopEffect>, IUpdateable
{
    private IHitstopComponent Hitstop { get; }
    private ITransformComponent Transform { get; }
    private ITimeManager TimeManager { get; }
    private ISpriteComponent? Sprite { get; }

    public PlayerHitstopHandler(IEffectsComponent effects, IHitstopComponent hitstop, ITransformComponent transform,
        ITimeManager timeManager, ISpriteComponent? sprite = null) : base(effects)
    {
        Hitstop = hitstop;
        Transform = transform;
        TimeManager = timeManager;
        Sprite = sprite;
    }

    protected override void HandleInternal(HitstopEffect effect)
    {
        Hitstop.RemainingHitstopTime = effect.Duration;
        
        if (Hitstop.IsStopped) { return; }
        
        // Transform.StopMotion();
        Sprite?.StopAnimation();

        Hitstop.IsStopped = true;
        
        // Hitstop.Stop();
    }

    public void Activity()
    {
        Hitstop.RemainingHitstopTime -= TimeManager.GameTimeSinceLastFrame;
        
        if (Hitstop.IsStopped && Hitstop.RemainingHitstopTime <= TimeSpan.Zero)
        {
            // Transform.ResumeMotion();
            Sprite?.ResumeAnimation();
            
            Hitstop.IsStopped = false;
            
            // Hitstop.Resume();
        }
    }
}