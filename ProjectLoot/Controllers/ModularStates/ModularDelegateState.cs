using System.Collections.Generic;
using ANLG.Utilities.States;

namespace ProjectLoot.Controllers.ModularStates;

public class ModularDelegateState : IState
{
    private Action? _activate;
    private Action? _activity;
    private Func<IState?>? _exitConditions;
    private Action? _deactivate;

    protected T AddModule<T>(T module) where T : class, IModule
    {
        bool added = false;
        if (module is IActivate activate)
        {
            _activate += activate.OnActivate;
            added = true;
        }
        
        if (module is IActivity activity)
        {
            _activity += activity.CustomActivity;
            added = true;
        }
        
        if (module is IExitCondition exitCondition)
        {
            _exitConditions += exitCondition.EvaluateExitConditions;
            added = true;
        }
        
        if (module is IDeactivate deactivate)
        {
            _deactivate += deactivate.BeforeDeactivate;
            added = true;
        }

        if (!added)
        {
            throw new ArgumentException("Unrecognized module type");
        }

        return module;
    }
    
    protected void AddActivate(Action             d) => _activate += d;
    protected void AddActivity(Action             d) => _activity += d;
    protected void AddExitCondition(Func<IState?> d) => _exitConditions += d;
    protected void AddDeactivate(Action           d) => _deactivate += d;

    public void OnActivate()
    {
        _activate?.Invoke();
    }

    public void CustomActivity()
    {
        _activity?.Invoke();
    }

    public IState? EvaluateExitConditions()
    {
        if (_exitConditions is null)
        {
            return null;
        }
        
        foreach (Func<IState?> exitCondition in _exitConditions.GetInvocationList())
        {
            IState? result = exitCondition();
            if (result is not null)
            {
                return result;
            }
        }

        return null;
    }

    public void BeforeDeactivate()
    {
        _deactivate?.Invoke();
    }
}
