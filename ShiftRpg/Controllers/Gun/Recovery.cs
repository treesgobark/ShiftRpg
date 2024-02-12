using ShiftRpg.Contracts;

namespace ShiftRpg.Controllers.Gun;

public class Recovery(IGun obj) : GunController(obj)
{
    protected GunController? NextState { get; set; }
    
    public override void Initialize()
    {
    }

    public override void CustomActivity()
    {
        if (Parent.InputDevice.Reload.WasJustPressed)
        {
            NextState = Get<Reloading>();
        }
    }

    public override GunController? EvaluateExitConditions()
    {
        if (NextState is not null)
        {
            return NextState;
        }

        if (TimeInState > Parent.TimePerRound.TotalSeconds)
        {
            return Get<Ready>();
        }

        return null;
    }

    public override void BeforeDeactivate()
    {
        NextState = null;
    }
}