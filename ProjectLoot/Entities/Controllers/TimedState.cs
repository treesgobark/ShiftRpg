using ANLG.Utilities.FlatRedBall.States;
using FlatRedBall;

namespace ProjectLoot.Entities;

public abstract class TimedState<T> : State<T>
{
    protected TimedState(T parent, IReadonlyStateMachine stateMachine) : base(parent, stateMachine) { }
    
    private double StartTime { get; set; }
    protected double TimeInState { get; set; }
    
    public override void OnActivate()
    {
        StartTime = TimeManager.CurrentScreenTime;
        TimeInState = 0;
        AfterTimedStateActivate();
    }

    public override void CustomActivity()
    {
        TimeInState += TimeManager.SecondDifference;
        AfterTimedStateActivity();
    }

    protected abstract void AfterTimedStateActivate();
    protected abstract void AfterTimedStateActivity();
}