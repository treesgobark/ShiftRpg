using ANLG.Utilities.Core;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.Effects;
using ProjectLoot.Handlers.Base;

namespace ProjectLoot.Handlers;

public class HitstopHandler : EffectHandler<HitstopEffect>, IUpdateable
{
    private readonly IHitstopComponent _hitstop;
    private readonly ITransformComponent _transform;
    private readonly ITimeManager _timeManager;
    private readonly ISpriteComponent? _sprite;
    private readonly IHealthComponent? _health;

    public HitstopHandler(IEffectsComponent effects, IHitstopComponent hitstop, ITransformComponent transform,
                          ITimeManager timeManager, ISpriteComponent? sprite = null, IHealthComponent? health = null) : base(effects)
    {
        _hitstop     = hitstop;
        _transform   = transform;
        _timeManager = timeManager;
        _sprite      = sprite;
        _health      = health;
    }

    public override void Handle(HitstopEffect effect)
    {
        _hitstop.RemainingHitstopTime = effect.Duration;
        
        if (_hitstop.IsStopped) { return; }
        
        _transform.StopMotion();
        _sprite?.StopAnimation();

        _hitstop.IsStopped = true;
        
        _hitstop.Stop();
    }

    public void Activity()
    {
        _hitstop.RemainingHitstopTime -= _timeManager.GameTimeSinceLastFrame;
        
        if (_hitstop.IsStopped && _hitstop.RemainingHitstopTime <= TimeSpan.Zero)
        {
            _transform.ResumeMotion();
            _sprite?.ResumeAnimation();
            
            _hitstop.IsStopped = false;
            
            _hitstop.Resume();
        }
    }
}