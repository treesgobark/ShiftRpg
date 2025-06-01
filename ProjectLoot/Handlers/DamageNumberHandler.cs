using ANLG.Utilities.Core.NonStaticUtilities;
using FlatRedBall.Graphics;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.Effects;
using ProjectLoot.Factories;
using ProjectLoot.Handlers.Base;

namespace ProjectLoot.Handlers;

public class DamageNumberHandler : EffectHandler<HealthReductionEffect>
{
    private readonly IEffectsComponent _effects;
    private readonly ITransformComponent _transform;

    public DamageNumberHandler(IEffectsComponent effects, ITransformComponent transform) : base(effects)
    {
        _effects   = effects;
        _transform = transform;
    }

    public override void Handle(HealthReductionEffect effect)
    {
        DamageNumberFactory.CreateNew()
                           .SetStartingValues(effect.Value, 1, _transform.Position, effect.Source, _effects.Team);
    }
}
