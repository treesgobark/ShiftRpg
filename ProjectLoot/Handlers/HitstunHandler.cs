using ANLG.Utilities.Core.NonStaticUtilities;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;

namespace ProjectLoot.Effects.Handlers;

public class HitstunHandler : EffectHandler<HitstunEffect>, IUpdateable
{
    private IHitstunComponent Hitstun { get; }
    private IHitstopComponent Hitstop { get; }
    private ITimeManager TimeManager { get; }

    public HitstunHandler(IEffectsComponent effects, IHitstunComponent hitstun, IHitstopComponent hitstop, ITimeManager timeManager) : base(effects)
    {
        Hitstun = hitstun;
        Hitstop = hitstop;
        TimeManager = timeManager;
    }

    protected override void Handle(HitstunEffect effect)
    {
        if (!Effects.Team.IsSubsetOf(effect.AppliesTo)) { return; }

        Hitstun.RemainingHitstunDuration = effect.Duration;
        
        Hitstun.IsStunned = true;
    }

    public void Activity()
    {
        if (Hitstop.IsStopped)
        {
            return;
        }
        
        switch (Hitstun)
        {
            case { IsStunned: false, RemainingHitstunDuration.TotalSeconds: <= 0 }:
                return;
            case { IsStunned: false, RemainingHitstunDuration.TotalSeconds: > 0 }:
                OnStunBegins();
                break;
            case { IsStunned: true, RemainingHitstunDuration.TotalSeconds: > 0 }:
                Hitstun.RemainingHitstunDuration -= TimeSpan.FromSeconds(TimeManager.GameTimeSinceLastFrame.TotalSeconds);
                break;
            case { IsStunned: true, RemainingHitstunDuration.TotalSeconds: <= 0 }:
                OnStunEnds();
                break;
        }
    }

    private void OnStunBegins()
    {
        Hitstun.IsStunned = true;
    }

    private void OnStunEnds()
    {
        Hitstun.IsStunned = false;
    }
}