using FlatRedBall;
using ShiftRpg.Contracts;

namespace ShiftRpg.Controllers.Gun;

public class Reloading(IGun obj) : GunController(obj)
{
    public override void Initialize() { }

    public override void OnActivate()
    {
        Parent.StartReload();
        base.OnActivate();
    }

    public override void CustomActivity() { }

    public override GunController? EvaluateExitConditions()
    {
        if (TimeInState > Parent.ReloadTime.TotalSeconds)
        {
            return Get<Ready>();
        }

        return null;
    }

    public override void BeforeDeactivate()
    {
        Parent.FillMagazine();
    }
}