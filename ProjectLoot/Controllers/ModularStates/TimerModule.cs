using ANLG.Utilities.Core;
using ANLG.Utilities.States;

namespace ProjectLoot.Controllers.ModularStates;


public class TimerModule : ITimerModule, IActivate, IUpdate
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

    public virtual void Update()
    {
        TimeInState += _timeManager.GameTimeSinceLastFrame;
    }
}
