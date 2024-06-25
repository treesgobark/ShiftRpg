using FlatRedBall;
using Microsoft.Xna.Framework;
using ProjectLoot.Components.Interfaces;

namespace ProjectLoot.Components;

public class TransformComponent : ITransformComponent
{
    private Vector3 _cachedVelocity;
    private Vector3 _cachedAcceleration;
    private int _stopCount;

    private PositionedObject PositionedObject { get; }

    public TransformComponent(PositionedObject positionedObject)
    {
        PositionedObject = positionedObject;
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
        get => PositionedObject.Velocity;
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
        get => PositionedObject.Acceleration;
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
