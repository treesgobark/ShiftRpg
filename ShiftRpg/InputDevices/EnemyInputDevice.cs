using FlatRedBall;
using FlatRedBall.Input;
using ShiftRpg.Entities;

namespace ShiftRpg.InputDevices;

public class EnemyInputDevice<T> : IInputDevice
    where T : Enemy
{
    private readonly T _owner;
    private PositionedObject? _target;
    private ZeroInputDevice _zeroInput = new();

    public EnemyInputDevice(T owner)
    {
        _owner = owner;
        Default2DInput              = _zeroInput.Default2DInput;
        DefaultUpPressable          = _zeroInput.DefaultUpPressable;
        DefaultDownPressable        = _zeroInput.DefaultDownPressable;
        DefaultLeftPressable        = _zeroInput.DefaultLeftPressable;
        DefaultRightPressable       = _zeroInput.DefaultRightPressable;
        DefaultHorizontalInput      = _zeroInput.DefaultHorizontalInput;
        DefaultVerticalInput        = _zeroInput.DefaultVerticalInput;
        DefaultPrimaryActionInput   = _zeroInput.DefaultPrimaryActionInput;
        DefaultSecondaryActionInput = _zeroInput.DefaultSecondaryActionInput;
        DefaultConfirmInput         = _zeroInput.DefaultConfirmInput;
        DefaultJoinInput            = _zeroInput.DefaultJoinInput;
        DefaultPauseInput           = _zeroInput.DefaultPauseInput;
        DefaultBackInput            = _zeroInput.DefaultBackInput;
        DefaultCancelInput          = _zeroInput.DefaultCancelInput;
    }

    public PositionedObject? Target
    {
        get => _target;
        set
        {
            _target        = value;
            Default2DInput = _target is not null
                ? new EntityTracker(_owner, _target)
                : _zeroInput.Default2DInput;
        }
    }

    public I2DInput Default2DInput { get; private set; }
    public IRepeatPressableInput DefaultUpPressable { get; }
    public IRepeatPressableInput DefaultDownPressable { get; }
    public IRepeatPressableInput DefaultLeftPressable { get; }
    public IRepeatPressableInput DefaultRightPressable { get; }
    public I1DInput DefaultHorizontalInput { get; }
    public I1DInput DefaultVerticalInput { get; }
    public IPressableInput DefaultPrimaryActionInput { get; }
    public IPressableInput DefaultSecondaryActionInput { get; }
    public IPressableInput DefaultConfirmInput { get; }
    public IPressableInput DefaultJoinInput { get; }
    public IPressableInput DefaultPauseInput { get; }
    public IPressableInput DefaultBackInput { get; }
    public IPressableInput DefaultCancelInput { get; }
}