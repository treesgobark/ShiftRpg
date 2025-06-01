using ANLG.Utilities.Core.NonStaticUtilities;
using FlatRedBall.Graphics;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.Effects;
using ProjectLoot.Factories;
using ProjectLoot.Handlers.Base;

namespace ProjectLoot.Handlers;

public class HealthReductionHandler : EffectHandler<HealthReductionEffect>
{
    private readonly IEffectsComponent _effects;
    private readonly IHealthComponent _health;
    private readonly ITimeManager _timeManager;

    public HealthReductionHandler(IEffectsComponent effects, IHealthComponent health, ITimeManager timeManager) : base(effects)
    {
        _effects     = effects;
        _health      = health;
        _timeManager = timeManager;
    }

    public override void Handle(HealthReductionEffect effect)
    {
        float finalDamage = effect.Value;
        
        _health.CurrentHealth  -= finalDamage;
        _health.LastDamageTime =  _timeManager.TotalGameTime;

        if (_health.CurrentHealth <= 0)
        {
            _effects.Handle(new DeathEffect(effect.AppliesTo, effect.Source));
        }
    }
}
