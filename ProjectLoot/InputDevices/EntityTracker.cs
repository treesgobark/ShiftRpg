using ANLG.Utilities.FlatRedBall.Extensions;
using FlatRedBall;
using FlatRedBall.Input;
using Microsoft.Xna.Framework;

namespace ProjectLoot.InputDevices;

public class EntityTracker : I2DInput
{
    private readonly PositionedObject _thisEntity;
    private PositionedObject _trackedEntity;

    public EntityTracker(PositionedObject thisEntity, PositionedObject trackedEntity)
    {
        _thisEntity = thisEntity;
        _trackedEntity = trackedEntity;
    }
    
    public void SetTarget(PositionedObject target)
    {
        _trackedEntity = target;
    }
    
    public void ClearTarget()
    {
        _trackedEntity = _thisEntity;
    }

    public float X => ToTrackedEntity.X;
    public float Y => ToTrackedEntity.Y;
    public float XVelocity => 64;
    public float YVelocity => 64;
    public float Magnitude => ToTrackedEntity.Length();
    
    public float Distance2D => _thisEntity.Position
        .GetVectorTo(_trackedEntity.Position)
        .XY()
        .Length();

    private Vector2 ToTrackedEntity => _thisEntity.Position
        .GetVectorTo(_trackedEntity.Position)
        .XY()
        .NormalizedOrZero();
}