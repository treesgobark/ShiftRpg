using FlatRedBall;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.Effects;
using ProjectLoot.Handlers.Base;

namespace ProjectLoot.Handlers;

public class WeaknessDamageHandler : EffectHandler<WeaknessDamageEffect>, IUpdateable
{
    private readonly IEffectsComponent _effects;
    private readonly IWeaknessComponent _weakness;

    public WeaknessDamageHandler(IEffectsComponent effects, IWeaknessComponent weakness) : base(effects)
    {
        _effects  = effects;
        _weakness = weakness;
    }

    public override void Handle(WeaknessDamageEffect effect)
    {
        bool valid = ValidateEffect(effect);
        if (!valid) { return; }
        
        float finalDamage = effect.WeaknessDamage;
        
        ApplyDamageModifiers(effect, ref finalDamage);
        ApplyDamage(effect, finalDamage);
    }
    
    protected virtual bool ValidateEffect(WeaknessDamageEffect effect)
    {
        if (!_effects.Team.IsSubsetOf(effect.AppliesTo)) { return false; }

        return true;
    }
    
    protected virtual void ApplyDamageModifiers(WeaknessDamageEffect damageEffect, ref float finalDamage)
    {
        finalDamage = (damageEffect.AdditiveIncreases.Sum() + 1) * finalDamage;
        
        if (damageEffect.MultiplicativeIncreases.Count > 0)
        {
            finalDamage = damageEffect.MultiplicativeIncreases.Aggregate((f1, f2) => f1 * f2) * finalDamage;
        }
    }
    
    protected virtual void ApplyDamage(WeaknessDamageEffect effect, float finalDamage)
    {
        _weakness.CurrentWeaknessPercentage += finalDamage * 100f;
    }

    public void Activity()
    {
        if (_weakness.CurrentWeaknessPercentage > 0)
        {
            _weakness.CurrentWeaknessPercentage -= TimeManager.SecondDifference * _weakness.DepletionRatePerSecond;
        }
    }
}