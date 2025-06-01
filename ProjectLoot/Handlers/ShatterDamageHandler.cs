using ProjectLoot.Components.Interfaces;
using ProjectLoot.Effects;
using ProjectLoot.Handlers.Base;

namespace ProjectLoot.Handlers;

public class ShatterDamageHandler : EffectHandler<ShatterDamageEffect>
{
    private readonly IEffectsComponent _effects;
    private readonly IHealthComponent _health;
    private readonly IShatterComponent _shatter;

    public ShatterDamageHandler(IEffectsComponent effects, IHealthComponent health, IShatterComponent shatter) : base(effects)
    {
        _effects = effects;
        _health        = health;
        _shatter       = shatter;
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
        if (!_effects.Team.IsSubsetOf(effect.AppliesTo)) { return false; }

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
        _shatter.SetShatterDamage(_shatter.CurrentShatterDamage + finalDamage, _health);
    }
}