using ANLG.Utilities.FlatRedBall.States;
using FlatRedBall;

namespace ProjectLoot.Entities;

public abstract class TimedState<T> : State<T>
{
    protected TimedState(T parent, IReadonlyStateMachine stateMachine) : base(parent, stateMachine) { }
    
    protected double TimeInState { get; set; }
    
    public override void OnActivate()
    {
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