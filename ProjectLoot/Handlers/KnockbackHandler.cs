using Microsoft.Xna.Framework;
using ProjectLoot.Components.Interfaces;

namespace ProjectLoot.Effects.Handlers;

public class KnockbackHandler : EffectHandler<KnockbackEffect>
{
    private ITransformComponent Transform { get; }

    public KnockbackHandler(IEffectsComponent effects, ITransformComponent transform) : base(effects)
    {
        Transform = transform;
    }

    protected override void Handle(KnockbackEffect effect)
    {
        if (!Effects.Team.IsSubsetOf(effect.AppliesTo)) { return; }

        switch (effect)
        {
            case { KnockbackBehavior: KnockbackBehavior.Additive }:
                Transform.Velocity += effect.KnockbackVector;
                break;
            case { KnockbackBehavior: KnockbackBehavior.Replacement }:
                Transform.Velocity = effect.KnockbackVector;
                break;
        }
        
        if (Transform.CurrentSpeed > Transform.MaxSpeed && Transform.DecelerationAboveMaxSpeed > 0)
        {
            Transform.Acceleration = Transform.DecelerationAboveMaxSpeed * -Transform.Velocity.Normalized();
        }
    }
}