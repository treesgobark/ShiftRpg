using FlatRedBall;
using Microsoft.Xna.Framework;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.TopDown;

namespace ProjectLoot.Components;

public class TransformComponent : ITransformComponent
{
    private Vector3 _cachedVelocity;
    private Vector3 _cachedAcceleration;
    private int _stopCount;

    private PositionedObject PositionedObject { get; }
    private ITopDownEntity? TopDownEntity { get; }

    public TransformComponent(PositionedObject positionedObject, ITopDownEntity? topDownEntity = null)
    {
        PositionedObject   = positionedObject;
        TopDownEntity = topDownEntity;
    }

    private int StopCount
    {
        get => _stopCount;
        set
        {
#if DEBUG
            if (value < 0)
            {
                throw new InvalidOperationException("Tried to resume motion when it wasn't already stopped.");
            }
#endif
            _stopCount = value;
        }
    }

    public Vector3 Position
    {
        get => PositionedObject.Position;
        set => PositionedObject.Position = value;
    }

    public Vector3 Velocity
    {
        get => IsStopped ? _cachedVelocity : PositionedObject.Velocity;
        set
        {
            if (!IsStopped)
            {
                PositionedObject.Velocity = value;
            }
            else
            {
                _cachedVelocity = value;
            }
        }
    }

    public Vector3 Acceleration
    {
        get => IsStopped ? _cachedAcceleration : PositionedObject.Acceleration;
        set
        {
            if (!IsStopped)
            {
                PositionedObject.Acceleration = value;
            }
            else
            {
                _cachedAcceleration = value;
            }
        }
    }

    public float CurrentSpeed => Velocity.Length();
    public float MaxSpeed => TopDownEntity.MaxSpeed;

    public float DecelerationAboveMaxSpeed => TopDownEntity.CurrentMovement.IsUsingCustomDeceleration
        ? TopDownEntity.CurrentMovement.CustomDecelerationValue
        : 0;

    private bool IsStopped => StopCount > 0;
    
    public void StopMotion()
    {
        if (StopCount == 0)
        {
            _cachedVelocity = PositionedObject.Velocity;
            _cachedAcceleration = PositionedObject.Acceleration;
            
            PositionedObject.Velocity = Vector3.Zero;
            PositionedObject.Acceleration = Vector3.Zero;
        }

        StopCount++;
    }

    public void ResumeMotion()
    {
        StopCount--;
        
        if (StopCount == 0)
        {
            PositionedObject.Velocity = _cachedVelocity;
            PositionedObject.Acceleration = _cachedAcceleration;
        }
    }
}
