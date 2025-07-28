using ANLG.Utilities.Core;
using ANLG.Utilities.States;

namespace ProjectLoot.Controllers.ModularStates;


public class TimerModule : IState, ITimerModule
{
    private readonly ITimeManager _timeManager;
    public TimeSpan TimeInState { get; private set; }

    public TimerModule(ITimeManager timeManager)
    {
        _timeManager = timeManager;
    }

    public virtual void OnActivate()
    {
        TimeInState = TimeSpan.Zero;
    }

    public virtual void CustomActivity()
    {
        TimeInState += _timeManager.GameTimeSinceLastFrame;
    }

    public virtual IState? EvaluateExitConditions() => null;

    public virtual void BeforeDeactivate()
    {
    }
}
