using ProjectLoot.Contracts;

namespace ProjectLoot.Effects.Handlers;

public abstract class EffectHandler<T> : IEffectHandler<T>
{
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

    public abstract void Handle(T effect);
}