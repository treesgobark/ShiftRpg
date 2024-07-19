using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;

namespace ProjectLoot.Effects.Handlers;

public class ApplyShatterDamageHandler : EffectHandler<ApplyShatterEffect>
{
    private IShatterComponent Shatter { get; }
    private IHealthComponent Health { get; }

    public ApplyShatterDamageHandler(IEffectsComponent effects, IShatterComponent shatter, IHealthComponent health) : base(effects)
    {
        Shatter = shatter;
        Health = health;
    }

    protected override void Handle(ApplyShatterEffect effect)
    {
        bool valid = ValidateEffect(effect);
        if (!valid) { return; }
        
         if (Shatter.CurrentShatterDamage > 0)
         {
             Effects.Handle(new DamageEffect(effect.AppliesTo, SourceTag.Shatter, Shatter.CurrentShatterDamage));
             Shatter.SetShatterDamage(0f, Health);
         }
    }
    
    protected virtual bool ValidateEffect(ApplyShatterEffect effect)
    {
        if (!Effects.Team.IsSubsetOf(effect.AppliesTo)) { return false; }

        return true;
    }
}
