using ProjectLoot.Components.Interfaces;
using ProjectLoot.Effects;
using ProjectLoot.Effects.Base;

namespace ProjectLoot.Handlers.Base;

public abstract class EffectHandler<T> : IEffectHandler<T> where T : IEffect
{
    private readonly IEffectsComponent _effects;

    public EffectHandler(IEffectsComponent effects)
    {
        _effects = effects;
    }
    
    public void Handle(IEffect effect)
    {
        if (!effect.AppliesTo.Contains(_effects.Team)) { return; }
        
        if (effect is T castedEffect)
        {
            Handle(castedEffect);
        }
        else
        {
            throw new ArgumentException("Invalid effect type", nameof(effect));
        }
    }

    public bool CanHandle(IEffect effect)
    {
        if (!effect.AppliesTo.Contains(_effects.Team))
        {
            return false;
        }

        if (effect is not T)
        {
            return false;
        }

        return true;
    }

    public bool IsActive { get; set; }

    public abstract void Handle(T effect);
}
