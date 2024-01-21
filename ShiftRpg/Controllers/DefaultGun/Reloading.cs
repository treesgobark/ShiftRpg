using FlatRedBall;
using FlatRedBall.Debugging;

namespace ShiftRpg.Controllers.DefaultGun;

public class Reloading(Entities.DefaultGun parent) : DefaultGunController(parent)
{
    protected double ReloadStartTime { get; set; }
    
    public override void OnActivate()
    {
        ReloadStartTime = TimeManager.CurrentScreenTime;
        Debugger.CommandLineWrite($"[{TimeManager.CurrentFrame}] Reloading!");
    }
    
    public override DefaultGunController? EvaluateExitConditions()
    {
        if (TimeManager.CurrentScreenSecondsSince(ReloadStartTime) > Parent.ReloadTimeSeconds)
        {
            return Get<DefaultGunController>();
        }

        return null;
    }

    public override void BeforeDeactivate()
    {
        Parent.MagazineRemaining = parent.MagazineSize;
    }

    public override void BeginFire() { }
    public override void EndFire() { }
    public override void Reload() { }
}