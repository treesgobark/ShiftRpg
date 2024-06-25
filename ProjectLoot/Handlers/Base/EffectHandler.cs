using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;

namespace ProjectLoot.Effects.Handlers;

public abstract class EffectHandler<T> : IEffectHandler
{
    protected IEffectsComponent Effects { get; }

    public EffectHandler(IEffectsComponent effects)
    {
        Effects = effects;
    }
    
    public void Handle(object effect)
    {
        if (effect is T castedEffect)
        {
            Handle(castedEffect);
        }
        else
        {
            throw new ArgumentException("Invalid effect type", nameof(effect));
        }
    }

    public bool IsActive { get; set; }

    protected abstract void Handle(T effect);
}
