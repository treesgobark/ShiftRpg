using ANLG.Utilities.FlatRedBall.Controllers;
using ANLG.Utilities.FlatRedBall.States;

namespace ShiftRpg.Entities;

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
        StateMachine.Add(new Idle(this, StateMachine));
        StateMachine.Add(new Startup(this, StateMachine));
        StateMachine.Add(new Active(this, StateMachine));
        StateMachine.Add(new Recovery(this, StateMachine));
        StateMachine.InitializeStartingState<Idle>();
    }
}