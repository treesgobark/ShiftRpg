using ProjectLoot.Components.Interfaces;
using ProjectLoot.Effects;
using ProjectLoot.Handlers.Base;

namespace ProjectLoot.Handlers;

public class PoiseDamageHandler : EffectHandler<PoiseDamageEffect>
{
    private readonly IPoiseComponent _poise;

    public PoiseDamageHandler(IEffectsComponent effects, IPoiseComponent poise) : base(effects)
    {
        _poise = poise;
    }
    
    public override void Handle(PoiseDamageEffect effect)
    {
        _poise.CurrentPoiseDamage += effect.PoiseDamage;
    }
}