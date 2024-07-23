using ProjectLoot.Components.Interfaces;
using ProjectLoot.Effects;
using ProjectLoot.Handlers.Base;

namespace ProjectLoot.Handlers;

public class PoiseDamageHandler : EffectHandler<PoiseDamageEffect>
{
    private IPoiseComponent Poise { get; }

    public PoiseDamageHandler(IEffectsComponent effects, IPoiseComponent poise) : base(effects)
    {
        Poise = poise;
    }
    
    protected override void HandleInternal(PoiseDamageEffect effect)
    {
        Poise.CurrentPoiseDamage += effect.PoiseDamage;
    }
}