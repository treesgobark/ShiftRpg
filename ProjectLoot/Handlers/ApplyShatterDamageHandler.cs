using ProjectLoot.Components.Interfaces;
using ProjectLoot.Effects;
using ProjectLoot.Handlers.Base;

namespace ProjectLoot.Handlers;

public class ApplyShatterDamageHandler : EffectHandler<ApplyShatterEffect>
{
    private readonly IEffectsComponent _effects;
    private readonly IShatterComponent _shatter;
    private readonly IHealthComponent _health;

    public ApplyShatterDamageHandler(IEffectsComponent effects, IShatterComponent shatter, IHealthComponent health) : base(effects)
    {
        _effects = effects;
        _shatter = shatter;
        _health  = health;
    }

    protected override void HandleInternal(ApplyShatterEffect effect)
    {
         if (_shatter.CurrentShatterDamage > 0)
         {
             _effects.Handle(new AttackEffect(effect.AppliesTo, SourceTag.Shatter, _shatter.CurrentShatterDamage));
             _shatter.SetShatterDamage(0f, _health);
         }
    }
}
