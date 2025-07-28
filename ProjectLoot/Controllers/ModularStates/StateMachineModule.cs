using ANLG.Utilities.States;

namespace ProjectLoot.Controllers.ModularStates;

public class StateMachineModule<TStartingState, TNextState> : IState
    where TStartingState : IState
    where TNextState : IState
{
    public StateMachineModule(IReadonlyStateMachine outerStates)
    {
        OuterStates = outerStates;
    }
    
    public IStateMachine Substates { get; } = new StateMachine();
    public IReadonlyStateMachine OuterStates { get; }
    
    public void OnActivate()
    {
        Substates.SetStartingState<TStartingState>();
        Substates.AdvanceCurrentState();
    }

    public void CustomActivity()
    {
        Substates.DoCurrentStateActivity();
    }

    public IState? EvaluateExitConditions()
    {
        if (!Substates.IsRunning)
        {
            return OuterStates.Get<TNextState>();
        }

        return null;
    }

    public void BeforeDeactivate()
    {
        Substates.ShutDown();
    }
}