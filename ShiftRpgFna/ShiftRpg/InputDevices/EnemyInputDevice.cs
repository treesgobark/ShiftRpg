using FlatRedBall;
using FlatRedBall.Input;
using ShiftRpg.Contracts;
using ShiftRpg.Entities;

namespace ShiftRpg.InputDevices;

public class EnemyInputDevice : InputDeviceBase, IGameplayInputDevice
{
    protected Enemy Owner { get; }
    protected EntityTracker? EntityTracker { get; set; }

    public EnemyInputDevice(Enemy owner)
    {
        Owner = owner;
    }
    
    public float DistanceToEntity => EntityTracker?.Distance2D ?? float.MaxValue;
    public bool IsTracking => EntityTracker is not null;

    public void SetTarget(PositionedObject target)
    {
        EntityTracker = new EntityTracker(Owner, target);
    }

    public void ClearTarget()
    {
        EntityTracker = null;
    }

    protected override float GetDefault2DInputX() => EntityTracker?.X ?? 0;
    protected override float GetDefault2DInputY() => EntityTracker?.Y ?? 0;
    public I2DInput Movement => ((IInputDevice)this).Default2DInput;
    public I2DInput Aim => EntityTracker is not null ? EntityTracker : Zero2DInput.Instance;
    public IPressableInput Attack => FalsePressableInput.Instance;
    public IPressableInput Reload => FalsePressableInput.Instance;
    public IPressableInput Dash => FalsePressableInput.Instance;
    public IPressableInput QuickSwapWeapon => FalsePressableInput.Instance;
    public IPressableInput NextWeapon => FalsePressableInput.Instance;
    public IPressableInput PreviousWeapon => FalsePressableInput.Instance;
}

public class RangedEnemyInputDevice : EnemyInputDevice
{
    public float FollowDistance { get; set; }
    public float Tolerance { get; set; }

    public RangedEnemyInputDevice(Enemy owner, float followDistance, float tolerance) : base(owner)
    {
        FollowDistance = followDistance;
        Tolerance = tolerance;
    }
    
    public float MinDistance => FollowDistance - Tolerance;
    public float MaxDistance => FollowDistance + Tolerance;
    public bool WithinRange => EntityTracker?.Distance2D >= MinDistance && EntityTracker.Distance2D <= MaxDistance;

    protected override float GetDefault2DInputX()
    {
        float direction = 0;
        if (DistanceToEntity < MinDistance)
        {
            direction = -1;
        }
        else if (DistanceToEntity > MaxDistance)
        {
            direction = 1;
        }
        return base.GetDefault2DInputX() * direction;
    }

    protected override float GetDefault2DInputY()
    {
        float direction = 0;
        if (DistanceToEntity < MinDistance)
        {
            direction = -1;
        }
        else if (DistanceToEntity > MaxDistance)
        {
            direction = 1;
        }
        return base.GetDefault2DInputY() * direction;
    }
}
