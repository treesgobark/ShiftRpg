using System.Collections.Generic;
using ANLG.Utilities.FlatRedBall.Extensions;
using FlatRedBall;
using FlatRedBall.Math;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;

namespace ProjectLoot.Effects.Handlers;

public class HitstopHandler : EffectHandler<HitstopEffect>, IPersistentEffectHandler
{
    private IEffectsComponent Effects { get; }
    private IHitstopComponent Hitstop { get; }
    private IPositionable Position { get; }
    private Sprite? HandledSprite { get; }

    public HitstopHandler(IEffectsComponent effects, IHitstopComponent hitstop, IPositionable position, Sprite? sprite = null)
    {
        Effects = effects;
        Hitstop = hitstop;
        Position = position;
        HandledSprite = sprite;
    }
    
    public override void Handle(HitstopEffect effect)
    {
        if (!Effects.Team.IsSubsetOf(effect.AppliesTo)) { return; }

        Hitstop.HitstopEndTime = TimeManager.CurrentScreenTime + effect.Duration.TotalSeconds;
        
        if (Hitstop.IsStopped) { return; }
        
        Hitstop.StoredVelocity = Position.VelocityAsVec3();
        Hitstop.StoredAcceleration = Position.AccelerationAsVec3();
        
        Position.XVelocity = 0;
        Position.YVelocity = 0;
        Position.ZVelocity = 0;
        
        Position.XAcceleration = 0;
        Position.YAcceleration = 0;
        Position.ZAcceleration = 0;

        if (HandledSprite is not null)
        {
            Hitstop.StoredAnimationSpeed = HandledSprite.AnimationSpeed;
            HandledSprite.AnimationSpeed = 0;
        }

        Hitstop.IsStopped = true;
        
        Hitstop.Stop();
    }

    public void Activity()
    {
        if (Hitstop.IsStopped && TimeManager.CurrentScreenSecondsSince(Hitstop.HitstopEndTime) >= 0)
        {
            Position.XVelocity = Hitstop.StoredVelocity.X;
            Position.YVelocity = Hitstop.StoredVelocity.Y;
            Position.ZVelocity = Hitstop.StoredVelocity.Z;
        
            Position.XAcceleration = Hitstop.StoredAcceleration.X;
            Position.YAcceleration = Hitstop.StoredAcceleration.Y;
            Position.ZAcceleration = Hitstop.StoredAcceleration.Z;

            if (HandledSprite is not null)
            {
                HandledSprite.AnimationSpeed = Hitstop.StoredAnimationSpeed;
            }
            
            Hitstop.IsStopped = false;
            
            Hitstop.Resume();
        }
    }
}