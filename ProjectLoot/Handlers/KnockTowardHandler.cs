using ANLG.Utilities.Core;
using ANLG.Utilities.FlatRedBall.Extensions;
using Microsoft.Xna.Framework;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Effects;
using ProjectLoot.Handlers.Base;

namespace ProjectLoot.Handlers;

public class KnockTowardHandler : EffectHandler<KnockTowardEffect>
{
    private readonly ITransformComponent _transform;
    
    public KnockTowardHandler(IEffectsComponent effects, ITransformComponent transform) : base(effects)
    {
        _transform = transform;
    }

    public override void Handle(KnockTowardEffect effect)
    {
        var directionVector = _transform.Position.GetVectorTo(effect.TargetPosition).Normalized();
        _transform.Velocity += directionVector * effect.Strength;
    }
}