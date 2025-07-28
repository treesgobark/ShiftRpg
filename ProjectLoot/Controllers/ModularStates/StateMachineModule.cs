using System.Diagnostics;
using ANLG.Utilities.States;

namespace ProjectLoot.Controllers.ModularStates;

public class StateMachineModule : IState
{
    private Type? _startingType;
    private Type? _exitType;
    
    public StateMachineModule(IReadonlyStateMachine outerStates)
    {
        OuterStates = outerStates;
    }
    
    public IStateMachine Substates { get; } = new StateMachine();
    public IReadonlyStateMachine OuterStates { get; }

    public StateMachineModule AddSubstate(Func<IReadonlyStateMachine, IState> stateBuilder)
    {
        Substates.Add(stateBuilder(Substates));
        return this;
    }

    public StateMachineModule SetStartingState<T>() where T : IState
    {
        _startingType = typeof(T);
        return this;
    }

    public StateMachineModule SetExitState<T>() where T : IState
    {
        _exitType = typeof(T);
        return this;
    }
    
    public void OnActivate()
    {
        Debug.Assert(_startingType != null, "Starting state must be set before activating");
        Debug.Assert(_exitType != null, "Exit state must be set before activating");
        Substates.SetStartingState(Substates.Get(_startingType));
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
            return OuterStates.Get(_exitType);
        }

        return null;
    }

    public void BeforeDeactivate()
    {
        Substates.ShutDown();
    }
}