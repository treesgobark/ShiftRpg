using ANLG.Utilities.Core.NonStaticUtilities;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.Effects;
using ProjectLoot.Handlers.Base;

namespace ProjectLoot.Handlers;

public class HitstunHandler : EffectHandler<HitstunEffect>, IUpdateable
{
    private readonly IHitstunComponent _hitstun;
    private readonly IHitstopComponent _hitstop;
    private readonly ITimeManager _timeManager;

    public HitstunHandler(IEffectsComponent effects, IHitstunComponent hitstun, IHitstopComponent hitstop, ITimeManager timeManager) : base(effects)
    {
        _hitstun = hitstun;
        _hitstop = hitstop;
        _timeManager = timeManager;
    }

    public override void Handle(HitstunEffect effect)
    {
        _hitstun.RemainingHitstunDuration = effect.Duration;
        
        _hitstun.IsStunned = true;
    }

    public void Activity()
    {
        if (_hitstop.IsStopped)
        {
            return;
        }
        
        switch (_hitstun)
        {
            case { IsStunned: false, RemainingHitstunDuration.TotalSeconds: <= 0 }:
                return;
            case { IsStunned: false, RemainingHitstunDuration.TotalSeconds: > 0 }:
                OnStunBegins();
                break;
            case { IsStunned: true, RemainingHitstunDuration.TotalSeconds: > 0 }:
                _hitstun.RemainingHitstunDuration -= TimeSpan.FromSeconds(_timeManager.GameTimeSinceLastFrame.TotalSeconds);
                break;
            case { IsStunned: true, RemainingHitstunDuration.TotalSeconds: <= 0 }:
                OnStunEnds();
                break;
        }
    }

    private void OnStunBegins()
    {
        _hitstun.IsStunned = true;
    }

    private void OnStunEnds()
    {
        _hitstun.IsStunned = false;
    }
}