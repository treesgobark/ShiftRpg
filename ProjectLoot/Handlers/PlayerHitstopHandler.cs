using ANLG.Utilities.Core.NonStaticUtilities;
using FlatRedBall.Screens;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.Effects;
using ProjectLoot.Handlers.Base;
using ProjectLoot.Screens;

namespace ProjectLoot.Handlers;

public class PlayerHitstopHandler : EffectHandler<HitstopEffect>, IUpdateable
{
    private readonly IHitstopComponent _hitstop;
    private readonly ITimeManager _timeManager;
    private readonly ISpriteComponent? _sprite;

    public PlayerHitstopHandler(IEffectsComponent effects, IHitstopComponent hitstop,
        ITimeManager timeManager, ISpriteComponent? sprite = null) : base(effects)
    {
        _hitstop = hitstop;
        _timeManager = timeManager;
        _sprite = sprite;
    }

    public override void Handle(HitstopEffect effect)
    {
        _hitstop.RemainingHitstopTime = effect.Duration;
        
        if (_hitstop.IsStopped) { return; }
        
        // Transform.StopMotion();
        _sprite?.StopAnimation();

        _hitstop.IsStopped = true;
        var screen = (GameScreen)ScreenManager.CurrentScreen;
        screen.ShakeCamera(effect.Duration, 3);
        
        // Hitstop.Stop();
    }

    public void Activity()
    {
        _hitstop.RemainingHitstopTime -= _timeManager.GameTimeSinceLastFrame;
        
        if (_hitstop.IsStopped && _hitstop.RemainingHitstopTime <= TimeSpan.Zero)
        {
            // Transform.ResumeMotion();
            _sprite?.ResumeAnimation();
            
            _hitstop.IsStopped = false;
            
            // Hitstop.Resume();
        }
    }
}