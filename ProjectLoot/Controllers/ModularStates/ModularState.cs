using System.Collections.Generic;
using ANLG.Utilities.States;

namespace ProjectLoot.Controllers.ModularStates;

public class ModularState : IState
{
    private readonly List<IActivate> _activates = [];
    private readonly List<IActivity> _activities = [];
    private readonly List<IExitCondition> _exitConditions = [];
    private readonly List<IDeactivate> _deactivates = [];

    protected T AddModule<T>(T module) where T : class, IModule
    {
        bool added = false;
        if (module is IActivate activate)
        {
            _activates.Add(activate);
            added = true;
        }
        
        if (module is IActivity activity)
        {
            _activities.Add(activity);
            added = true;
        }
        
        if (module is IExitCondition exitCondition)
        {
            _exitConditions.Add(exitCondition);
            added = true;
        }
        
        if (module is IDeactivate deactivate)
        {
            _deactivates.Add(deactivate);
            added = true;
        }

        if (!added)
        {
            throw new ArgumentException("Unrecognized module type");
        }

        return module;
    }

    public void OnActivate()
    {
        foreach (IActivate module in _activates)
        {
            module.OnActivate();
        }
    }

    public void CustomActivity()
    {
        foreach (IActivity module in _activities)
        {
            module.CustomActivity();
        }
    }

    public IState? EvaluateExitConditions()
    {
        foreach (IExitCondition module in _exitConditions)
        {
            IState? result = module.EvaluateExitConditions();
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }

    public void BeforeDeactivate()
    {
        foreach (IDeactivate module in _deactivates)
        {
            module.BeforeDeactivate();
        }
    }
}
