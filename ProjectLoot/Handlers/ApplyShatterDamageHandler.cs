using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.Handlers.Base;

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

    protected override void HandleInternal(ApplyShatterEffect effect)
    {
         if (Shatter.CurrentShatterDamage > 0)
         {
             Effects.Handle(new DamageEffect(effect.AppliesTo, SourceTag.Shatter, Shatter.CurrentShatterDamage));
             Shatter.SetShatterDamage(0f, Health);
         }
    }
}
