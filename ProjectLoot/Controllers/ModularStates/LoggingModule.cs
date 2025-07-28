using ANLG.Utilities.States;
using FlatRedBall;
using FlatRedBall.Debugging;

namespace ProjectLoot.Controllers.ModularStates;

public class LoggingModule : IState
{
    private readonly string _identifier;
    private readonly LogLevel _logLevel;

    public LoggingModule(string identifier, LogLevel logLevel = LogLevel.Default)
    {
        _identifier    = identifier;
        _logLevel = logLevel;
    }
    
    public void OnActivate()
    {
        Debugger.CommandLineWrite($"{TimeManager.CurrentFrame}: {_identifier} - OnActivate");
    }

    public void CustomActivity()
    {
        if (_logLevel is LogLevel.Verbose)
        {
            Debugger.CommandLineWrite($"{TimeManager.CurrentFrame}: {_identifier} - CustomActivity");
        }
    }

    public IState? EvaluateExitConditions()
    {
        return null;
    }

    public void BeforeDeactivate()
    {
        Debugger.CommandLineWrite($"{TimeManager.CurrentFrame}: {_identifier} - BeforeDeactivate");
    }
}

public enum LogLevel
{
    Default,
    Verbose,
}
