namespace ProjectLoot.Controllers.ModularStates;

public interface IDurationModule : ITimerModule
{
    TimeSpan Duration { get; }
    float NormalizedProgress { get; }
    bool HasDurationCompleted { get; }
}