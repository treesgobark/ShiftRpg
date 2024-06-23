using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;

namespace ProjectLoot.Effects.Handlers;

public abstract class EffectHandler<T> : IEffectHandler<T>
{
    protected IEffectsComponent Effects { get; }

    public EffectHandler(IEffectsComponent effects)
    {
        Effects = effects;
    }
    
    public void Handle(object effect)
    {
        if (Effects.IsInvulnerable)
        {
            return;
        }
        
        if (effect is T castedEffect)
        {
            Handle(castedEffect);
        }
        else
        {
            throw new ArgumentException("Invalid effect type", nameof(effect));
        }
    }

    public abstract void Handle(T effect);
}
