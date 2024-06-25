using ANLG.Utilities.Core.States;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;

namespace ProjectLoot.Entities;

public partial class DefaultSword
{
    protected StateMachine StateMachine { get; set; }
    
    /// <summary>
    /// Initialization logic which is executed only one time for this Entity (unless the Entity is pooled).
    /// This method is called when the Entity is added to managers. Entities which are instantiated but not
    /// added to managers will not have this method called.
    /// </summary>
    private void CustomInitialize()
    {
        InitializeControllers();
    }

    private void CustomActivity()
    {
        StateMachine.DoCurrentStateActivity();
    }

    private void CustomDestroy()
    {
    }

    private static void CustomLoadStaticContent(string contentManagerName)
    {
    }

    private void InitializeControllers()
    {
        StateMachine = new StateMachine();
        StateMachine.Add(new Idle(this, StateMachine, FrbTimeManager.Instance));
        StateMachine.Add(new Startup(this, StateMachine, FrbTimeManager.Instance));
        StateMachine.Add(new Active(this, StateMachine, FrbTimeManager.Instance));
        StateMachine.Add(new Recovery(this, StateMachine, FrbTimeManager.Instance));
        StateMachine.InitializeStartingState<Idle>();
    }
}