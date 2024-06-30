using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;

namespace ProjectLoot.Controllers;

public abstract class ParentedTimedState<T> : TimedState
{
    protected T Parent { get; }

    protected ParentedTimedState(IReadonlyStateMachine stateMachine, ITimeManager timeManager, T parent) : base(stateMachine, timeManager)
    {
        Parent = parent;
    }
}