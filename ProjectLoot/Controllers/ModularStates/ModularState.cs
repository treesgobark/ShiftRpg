using System.Collections.Generic;
using ANLG.Utilities.States;

namespace ProjectLoot.Controllers.ModularStates;

public class ModularState : IState
{
    private readonly List<IState> _modules = [];
    
    public T AddModule<T>(T module) where T : IState
    {
        _modules.Add(module);
        return module;
    }

    public void OnActivate()
    {
        foreach (IState module in _modules)
        {
            module.OnActivate();
        }
    }

    public void CustomActivity()
    {
        foreach (IState module in _modules)
        {
            module.CustomActivity();
        }
    }

    public IState? EvaluateExitConditions()
    {
        foreach (IState module in _modules)
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
        foreach (IState module in _modules)
        {
            module.BeforeDeactivate();
        }
    }
}
