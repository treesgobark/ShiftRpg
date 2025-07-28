using ANLG.Utilities.Core;
using ANLG.Utilities.States;

namespace ProjectLoot.Controllers;

public abstract class DurationState<T> : ParentedTimedState<T>
{
    public abstract TimeSpan Duration { get; }

    protected float NormalizedProgress => (float)(TimeInState / Duration).Saturate();
    
    protected bool HasDurationCompleted => NormalizedProgress >= 1;

    public DurationState(ITimeManager timeManager, T parent)
        : base(timeManager, parent) { }

    protected override void AfterTimedStateActivate()
    {
    }
}
