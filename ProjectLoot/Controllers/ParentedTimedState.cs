using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;

namespace ProjectLoot.Controllers;

public abstract class ParentedTimedState<T> : TimedState
{
    protected T Parent { get; }
    protected IReadonlyStateMachine StateMachine { get; }

    protected ParentedTimedState(T parent, IReadonlyStateMachine stateMachine, ITimeManager timeManager) : base(timeManager)
    {
        Parent = parent;
        StateMachine = stateMachine;
    }
}