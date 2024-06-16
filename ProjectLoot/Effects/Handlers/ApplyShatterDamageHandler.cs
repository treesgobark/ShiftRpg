using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;

namespace ProjectLoot.Effects.Handlers;

public class ApplyShatterDamageHandler : EffectHandler<ApplyShatterEffect>
{
    private IEffectsComponent Effects { get; }
    private IShatterComponent Shatter { get; }
    private IHealthComponent Health { get; }

    public ApplyShatterDamageHandler(IEffectsComponent effects, IShatterComponent shatter, IHealthComponent health)
    {
        Effects = effects;
        Shatter = shatter;
        Health = health;
    }
    
    public override void Handle(ApplyShatterEffect effect)
    {
        bool valid = ValidateEffect(effect);
        if (!valid) { return; }
        
         if (Shatter.CurrentShatterDamage > 0)
         {
             var bundle = new EffectBundle(effect.AppliesTo, SourceTag.Shatter);
             bundle.AddEffect(new DamageEffect(effect.AppliesTo, SourceTag.Shatter, Shatter.CurrentShatterDamage));
             Effects.HandlerCollection.Handle(bundle);
             Shatter.SetShatterDamage(0f, Health);
         }
    }
    
    protected virtual bool ValidateEffect(ApplyShatterEffect effect)
    {
        if (!Effects.Team.IsSubsetOf(effect.AppliesTo)) { return false; }
        if (Health.IsInvulnerable) { return false; }

        return true;
    }
}