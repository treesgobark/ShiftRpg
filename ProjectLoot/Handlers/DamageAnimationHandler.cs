using ProjectLoot.Components.Interfaces;
using ProjectLoot.Effects;
using ProjectLoot.Handlers.Base;
using IUpdateable = ProjectLoot.Contracts.IUpdateable;

namespace ProjectLoot.Handlers;

public class DamageAnimationHandler : EffectHandler<HealthReductionEffect>, IUpdateable
{
    private readonly IDamageableSpriteComponent _sprite;

    public DamageAnimationHandler(IEffectsComponent effects, IDamageableSpriteComponent sprite) : base(effects)
    {
        _sprite = sprite;
    }

    public override void Handle(HealthReductionEffect effect)
    {
        _sprite.PlayDamageAnimation();
    }

    public void Activity()
    {
        if (_sprite.RemainingAnimationTime > TimeSpan.Zero)
        {
            
        }
    }
}
