using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.StaticUtilities;
using FlatRedBall.Glue.StateInterpolation;
using Microsoft.Xna.Framework;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Effects;
using ProjectLoot.Handlers.Base;
using IUpdateable = ProjectLoot.Contracts.IUpdateable;

namespace ProjectLoot.Handlers;

public class KnockTowardHandler : EffectHandler<KnockTowardEffect>, IUpdateable
{
    private readonly ITransformComponent _transform;
    private readonly IHitstopComponent _hitstop;
    private readonly ITimeManager _timeManager;

    private Vector3 _startingPosition;
    private TimeSpan _remainingDuration;
    private KnockTowardEffect _currentEffect;
    
    private float NormalizedProgress => 1f - (float)(_remainingDuration / _currentEffect.Duration).Saturate();

    public KnockTowardHandler(IEffectsComponent effects, ITransformComponent transform, IHitstopComponent hitstop, ITimeManager timeManager) : base(effects)
    {
        _transform        = transform;
        _hitstop          = hitstop;
        _timeManager = timeManager;
    }

    protected override void HandleInternal(KnockTowardEffect effect)
    {
        _remainingDuration = effect.Duration;
        _startingPosition  = _transform.Position;
        _currentEffect     = effect;
    }

    public void Activity()
    {
        if (_hitstop.IsStopped)
        {
            return;
        }
        
        if (_remainingDuration <= TimeSpan.Zero)
        {
            return;
        }

        _transform.Position = Vector3.Lerp(_startingPosition, _currentEffect.TargetPosition, NormalizedProgress);
        
        _remainingDuration -= _timeManager.GameTimeSinceLastFrame;
    }
}