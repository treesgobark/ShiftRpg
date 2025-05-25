using System.Threading.Tasks;
using ANLG.Utilities.Core.NonStaticUtilities;
using Microsoft.Xna.Framework;
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

    protected override void HandleInternal(HealthReductionEffect effect)
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
