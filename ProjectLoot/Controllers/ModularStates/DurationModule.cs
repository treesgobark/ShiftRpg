using ANLG.Utilities.Core;
using ANLG.Utilities.States;

namespace ProjectLoot.Controllers.ModularStates;

public class DurationModule : TimerModule, IDurationModule
{
    public DurationModule(ITimeManager timeManager, TimeSpan duration) : base(timeManager)
    {
        Duration = duration;
    }

    public TimeSpan Duration { get; }
    
    public float NormalizedProgress => (float)(TimeInState / Duration).Saturate();
    public bool HasDurationCompleted => NormalizedProgress >= 1;
}

public class DurationExitModule<TNextState> : IExitCondition where TNextState : IState
{
    private readonly IDurationModule _durationModule;
    private readonly IReadonlyStateMachine _stateMachine;

    public DurationExitModule(IDurationModule durationModule, IReadonlyStateMachine stateMachine)
    {
        _durationModule = durationModule;
        _stateMachine   = stateMachine;
    }
    
    public IState? EvaluateExitConditions()
    {
        if (_durationModule.HasDurationCompleted)
        {
            return _stateMachine.Get<TNextState>();
        }

        return null;
    }
}