using ANLG.Utilities.Core;
using ANLG.Utilities.States;

namespace ProjectLoot.Controllers;

public abstract class ParentedTimedState<T> : TimedState
{
    protected T Parent { get; }

    protected ParentedTimedState(IReadonlyStateMachine states, ITimeManager timeManager, T parent) : base(states, timeManager)
    {
        Parent = parent;
    }
}