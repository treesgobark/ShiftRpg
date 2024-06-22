using FlatRedBall;
using FlatRedBall.Input;
using ProjectLoot.Contracts;
using ProjectLoot.Entities;

namespace ProjectLoot.InputDevices;

public class EnemyInputDevice : InputDeviceBase, IGameplayInputDevice
{
    private bool _inputEnabled = true;
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
        if (EntityTracker is not null)
        {
            EntityTracker.SetTarget(target);
        }
        else
        {
            EntityTracker = new EntityTracker(Owner, target);
        }
    }

    public void ClearTarget()
    {
        EntityTracker = null;
    }

    protected override float GetDefault2DInputX() => EntityTracker?.X ?? 0;
    protected override float GetDefault2DInputY() => EntityTracker?.Y ?? 0;
    public I2DInput Movement => ((IInputDevice)this).Default2DInput;
    public I2DInput Aim
    {
        get
        {
            if (!InputEnabled)
            {
                return ConstantAim;
            }
            return EntityTracker is not null ? EntityTracker : Zero2DInput.Instance;
        }
    }
    private Constant2DInput ConstantAim { get; set; }

    public IPressableInput Attack => TruePressableInput.Instance;
    public IPressableInput Reload => FalsePressableInput.Instance;
    public IPressableInput Dash => FalsePressableInput.Instance;
    public IPressableInput QuickSwapWeapon => FalsePressableInput.Instance;
    public IPressableInput NextWeapon => FalsePressableInput.Instance;
    public IPressableInput PreviousWeapon => FalsePressableInput.Instance;
    // public bool AimInMeleeRange => Aim.Magnitude < 1;
    public bool AimInMeleeRange => false;

    public bool InputEnabled
    {
        get => _inputEnabled;
        set
        {
            if ((_inputEnabled, value) is (true, false))
            {
                ConstantAim = new Constant2DInput(Aim);
            }
            _inputEnabled = value;
        }
    }
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
