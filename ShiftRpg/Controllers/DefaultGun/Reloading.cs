using FlatRedBall;
using FlatRedBall.Debugging;

namespace ShiftRpg.Controllers.DefaultGun;

public class Reloading(Entities.DefaultGun parent) : DefaultGunController(parent)
{
    private double ReloadStartTime { get; set; }
    private int BarColor { get; set; }
    
    public override void OnActivate()
    {
        ReloadStartTime = TimeManager.CurrentScreenTime;
        BarColor = parent.MagazineBar.ForegroundGreen;
        parent.MagazineBar.ForegroundGreen = 150;
    }

    public override void CustomActivity()
    {
        double progress = 100 * TimeManager.CurrentScreenSecondsSince(ReloadStartTime) / Parent.ReloadTimeSeconds;
        parent.MagazineBar.ProgressPercentage = (float)progress;
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
        parent.MagazineBar.ForegroundGreen = BarColor;
    }

    public override void BeginFire() { }
    public override void EndFire() { }
    public override void Reload() { }
}