using ANLG.Utilities.Core.NonStaticUtilities;
using Microsoft.Xna.Framework;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Effects;
using ProjectLoot.Handlers.Base;
using IUpdateable = ProjectLoot.Contracts.IUpdateable;

namespace ProjectLoot.Handlers;

public class FlashOnDamageHandler : EffectHandler<HealthReductionEffect>, IUpdateable
{
    private readonly ISpriteComponent _sprite;
    private readonly ITimeManager _timeManager;

    private TimeSpan _flashStarted;
    private TimeSpan _flashEnded;
    private static TimeSpan FlashDuration => TimeSpan.FromSeconds(0.2);

    public FlashOnDamageHandler(IEffectsComponent effects, ISpriteComponent sprite, ITimeManager timeManager) : base(effects)
    {
        _sprite           = sprite;
        _timeManager = timeManager;
    }

    protected override void HandleInternal(HealthReductionEffect effect)
    {
        _flashStarted = _timeManager.TotalGameTime;
        _sprite.Color = new Color(150, 150, 150, 255);
    }

    public void Activity()
    {
        if (_timeManager.TotalGameTime - _flashStarted <= FlashDuration || _flashEnded > _flashStarted)
        {
            return;
        }

        _flashEnded   = _timeManager.TotalGameTime;
        _sprite.Color = new Color(0, 0, 0, 255);
    }
}
