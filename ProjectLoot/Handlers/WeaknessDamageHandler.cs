using FlatRedBall;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;

namespace ProjectLoot.Effects.Handlers;

public class WeaknessDamageHandler : EffectHandler<WeaknessDamageEffect>, IUpdateable
{
    private IHealthComponent Health { get; }
    private IWeaknessComponent Weakness { get; }

    public WeaknessDamageHandler(IEffectsComponent effects, IHealthComponent health, IWeaknessComponent weakness) : base(effects)
    {
        Health = health;
        Weakness = weakness;
    }

    protected override void Handle(WeaknessDamageEffect effect)
    {
        bool valid = ValidateEffect(effect);
        if (!valid) { return; }
        
        float finalDamage = effect.WeaknessDamage;
        
        ApplyDamageModifiers(effect, ref finalDamage);
        ApplyDamage(effect, finalDamage);
    }
    
    protected virtual bool ValidateEffect(WeaknessDamageEffect effect)
    {
        if (!Effects.Team.IsSubsetOf(effect.AppliesTo)) { return false; }

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
        Weakness.CurrentWeaknessPercentage += finalDamage * 100f;
    }

    public void Activity()
    {
        if (Weakness.CurrentWeaknessPercentage > 0)
        {
            Weakness.CurrentWeaknessPercentage -= TimeManager.SecondDifference * Weakness.DepletionRatePerSecond;
        }
    }
}