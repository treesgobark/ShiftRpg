using ANLG.Utilities.Core.NonStaticUtilities;
using FlatRedBall.Graphics;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.Effects;
using ProjectLoot.Factories;
using ProjectLoot.Handlers.Base;

namespace ProjectLoot.Handlers;

public class HealthReductionHandler : EffectHandler<HealthReductionEffect>, IUpdateable
{
    private readonly IHealthComponent _health;
    private readonly ITimeManager _timeManager;
    private readonly IHitstopComponent _hitstop;
    private readonly IDestroyable? _destroyable;

    public HealthReductionHandler(IEffectsComponent effects, IHealthComponent health, ITimeManager timeManager,
                                  IHitstopComponent hitstop, IDestroyable?    destroyable = null) : base(effects)
    {
        _health       = health;
        _timeManager  = timeManager;
        _hitstop = hitstop;
        _destroyable  = destroyable;
    }

    protected override void HandleInternal(HealthReductionEffect effect)
    {
        float finalDamage = effect.Value;
        
        _health.CurrentHealth  -= finalDamage;
        _health.LastDamageTime =  _timeManager.TotalGameTime;
    }

    public void Activity()
    {
        if (!_hitstop.IsStopped && _health.CurrentHealth <= 0)
        {
            _destroyable?.Destroy();
        }
    }
}
