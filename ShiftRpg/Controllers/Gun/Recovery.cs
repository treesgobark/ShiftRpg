namespace ShiftRpg.Controllers.Gun;

public class Recovery(Entities.Gun obj) : GunController(obj)
{
    protected GunController? NextState { get; set; }
    
    public override void Initialize()
    {
    }

    public override void CustomActivity()
    {
    }

    public override GunController? EvaluateExitConditions()
    {
        if (NextState is not null)
        {
            return NextState;
        }

        if (TimeInState > Parent.CurrentGunData.SecondsPerRound)
        {
            return Get<Ready>();
        }

        return null;
    }

    public override void BeforeDeactivate()
    {
    }
}