using ANLG.Utilities.Core.NonStaticUtilities;
using FlatRedBall.Graphics;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.Effects;
using ProjectLoot.Effects.Base;
using ProjectLoot.Factories;
using ProjectLoot.Handlers.Base;

namespace ProjectLoot.Handlers;

public class CustomEventHandler<T> : EffectHandler<T> where T : IEffect
{
    private readonly Action<T> _action;

    public CustomEventHandler(IEffectsComponent effects, Action<T> action) : base(effects)
    {
        _action = action;
    }

    public override void Handle(T effect)
    {
        _action(effect);
    }
}
