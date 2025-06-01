using FlatRedBall.Graphics;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Effects;
using ProjectLoot.Handlers.Base;

namespace ProjectLoot.Handlers;

public class DestructionHandler : EffectHandler<DeathEffect>
{
    private readonly IDestroyable _destroyable;

    public DestructionHandler(IEffectsComponent effects, IDestroyable destroyable) : base(effects)
    {
        _destroyable = destroyable;
    }

    public override void Handle(DeathEffect effect)
    {
        _destroyable.Destroy();
    }
}