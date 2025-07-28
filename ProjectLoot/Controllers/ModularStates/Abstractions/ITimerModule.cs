using ANLG.Utilities.States;

namespace ProjectLoot.Controllers.ModularStates;

public interface ITimerModule
{
    public TimeSpan TimeInState { get; }
}