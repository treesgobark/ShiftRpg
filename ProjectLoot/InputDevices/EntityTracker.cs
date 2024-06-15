using ANLG.Utilities.FlatRedBall.Extensions;
using FlatRedBall;
using FlatRedBall.Input;
using Microsoft.Xna.Framework;

namespace ProjectLoot.InputDevices;

public class EntityTracker(PositionedObject thisEntity, PositionedObject trackedEntity) : I2DInput
{
    public float X => ToTrackedEntity.X;
    public float Y => ToTrackedEntity.Y;
    public float XVelocity => 64;
    public float YVelocity => 64;
    public float Magnitude => ToTrackedEntity.Length();
    
    public float Distance2D => thisEntity.Position
        .GetVectorTo(trackedEntity.Position)
        .XY()
        .Length();

    private Vector2 ToTrackedEntity => thisEntity.Position
        .GetVectorTo(trackedEntity.Position)
        .XY()
        .NormalizedOrZero();
}