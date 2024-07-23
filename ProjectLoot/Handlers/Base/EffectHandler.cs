using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.Effects;
using ProjectLoot.Effects.Base;

namespace ProjectLoot.Handlers.Base;

public abstract class EffectHandler<T> : IEffectHandler
{
    protected IEffectsComponent Effects { get; }

    public EffectHandler(IEffectsComponent effects)
    {
        Effects = effects;
    }
    
    public void Handle(IEffect effect)
    {
        if (!effect.AppliesTo.Contains(Effects.Team)) { return; }
        
        if (effect is T castedEffect)
        {
            HandleInternal(castedEffect);
        }
        else
        {
            throw new ArgumentException("Invalid effect type", nameof(effect));
        }
    }

    public bool IsActive { get; set; }

    protected abstract void HandleInternal(T effect);
}
