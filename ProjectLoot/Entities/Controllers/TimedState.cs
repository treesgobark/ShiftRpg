using ANLG.Utilities.FlatRedBall.States;
using FlatRedBall;

namespace ProjectLoot.Entities;

public abstract class TimedState<T> : State<T>
{
    protected TimedState(T parent, IReadonlyStateMachine stateMachine) : base(parent, stateMachine) { }
    
    private double StartTime { get; set; }
    protected double TimeInState => TimeManager.CurrentScreenSecondsSince(StartTime);
    
    public override void OnActivate()
    {
        StartTime = TimeManager.CurrentScreenTime;
        AfterTimedStateActivate();
    }

    protected abstract void AfterTimedStateActivate();
}