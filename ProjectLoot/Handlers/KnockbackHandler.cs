using System.Numerics;
using ProjectLoot.Components.Interfaces;

namespace ProjectLoot.Effects.Handlers;

public class KnockbackHandler : EffectHandler<KnockbackEffect>
{
    private ITransformComponent Transform { get; }
    private IHitstopComponent? Hitstop { get; }

    public KnockbackHandler(IEffectsComponent effects, ITransformComponent transform, IHitstopComponent? hitstop = null) : base(effects)
    {
        Transform = transform;
        Hitstop = hitstop;
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
    }
}