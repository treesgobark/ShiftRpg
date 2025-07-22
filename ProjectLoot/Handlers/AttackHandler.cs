using ANLG.Utilities.Core;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Effects;
using ProjectLoot.Handlers.Base;

namespace ProjectLoot.Handlers;

public class AttackHandler : EffectHandler<AttackEffect>
{
    private readonly IEffectsComponent _effects;
    private readonly IHealthComponent _health;
    private readonly ITimeManager _timeManager;

    public AttackHandler(IEffectsComponent effects, IHealthComponent health, ITimeManager timeManager) : base(effects)
    {
        _effects     = effects;
        _health      = health;
        _timeManager = timeManager;
    }

    public override void Handle(AttackEffect effect)
    {
        float finalDamage = effect.Value;
        
        ApplyDamageModifiers(effect, ref finalDamage);

        if (finalDamage > 0)
        {
            _health.LastDamageTime = _timeManager.TotalGameTime;
            _effects.Handle(new HealthReductionEffect(_effects.Team, effect.Source, finalDamage));
        }
    }

    private void ApplyDamageModifiers(AttackEffect attackEffect, ref float finalDamage)
    {
        _health.DamageModifiers.ModifyEffect(attackEffect);
        
        finalDamage = (int)((attackEffect.AdditiveIncreases.Sum() + 1) * finalDamage);
        
        if (attackEffect.MultiplicativeIncreases.Count > 0)
        {
            finalDamage = (int)(attackEffect.MultiplicativeIncreases.Aggregate((f1, f2) => f1 * f2) * finalDamage);
        }
    }
}
