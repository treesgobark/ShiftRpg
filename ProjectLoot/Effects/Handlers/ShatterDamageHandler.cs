using ProjectLoot.Components.Interfaces;

namespace ProjectLoot.Effects.Handlers;

public class ShatterDamageHandler : EffectHandler<ShatterDamageEffect>
{
    private IHealthComponent Health { get; }
    private IShatterComponent Shatter { get; }

    public ShatterDamageHandler(IEffectsComponent effects, IHealthComponent health, IShatterComponent shatter) : base(effects)
    {
        Health = health;
        Shatter = shatter;
    }
    
    public override void Handle(ShatterDamageEffect effect)
    {
        bool valid = ValidateEffect(effect);
        if (!valid) { return; }
        
        float finalDamage = effect.ShatterDamage;
        
        ApplyDamageModifiers(effect, ref finalDamage);
        ApplyDamage(effect, finalDamage);
    }
    
    protected virtual bool ValidateEffect(ShatterDamageEffect effect)
    {
        if (!Effects.Team.IsSubsetOf(effect.AppliesTo)) { return false; }

        return true;
    }
    
    protected virtual void ApplyDamageModifiers(ShatterDamageEffect damageEffect, ref float finalDamage)
    {
        finalDamage = (int)((damageEffect.AdditiveIncreases.Sum() + 1) * finalDamage);
        
        if (damageEffect.MultiplicativeIncreases.Count > 0)
        {
            finalDamage = (int)(damageEffect.MultiplicativeIncreases.Aggregate((f1, f2) => f1 * f2) * finalDamage);
        }
    }
    
    protected virtual void ApplyDamage(ShatterDamageEffect effect, float finalDamage)
    {
        Shatter.SetShatterDamage(Shatter.CurrentShatterDamage + finalDamage, Health);
    }
}