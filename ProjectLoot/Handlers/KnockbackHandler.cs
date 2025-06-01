using ProjectLoot.Components.Interfaces;
using ProjectLoot.Effects;
using ProjectLoot.Handlers.Base;

namespace ProjectLoot.Handlers;

public class KnockbackHandler : EffectHandler<KnockbackEffect>
{
    private readonly ITransformComponent _transform;

    public KnockbackHandler(IEffectsComponent effects, ITransformComponent transform) : base(effects)
    {
        _transform = transform;
    }

    public override void Handle(KnockbackEffect effect)
    {
        switch (effect)
        {
            case { KnockbackBehavior: KnockbackBehavior.Additive }:
                _transform.Velocity += effect.KnockbackVector;
                break;
            case { KnockbackBehavior: KnockbackBehavior.Replacement }:
                _transform.Velocity = effect.KnockbackVector;
                break;
        }
    }
}