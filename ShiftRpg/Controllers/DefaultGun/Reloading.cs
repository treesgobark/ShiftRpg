using FlatRedBall;
using FlatRedBall.Debugging;

namespace ShiftRpg.Controllers.DefaultGun;

public class Reloading(Entities.DefaultGun obj) : GunController(obj)
{
    private double ReloadStartTime { get; set; }
    private int BarColor { get; set; }

    public override void Initialize() { }

    public override void OnActivate()
    {
        ReloadStartTime = TimeManager.CurrentScreenTime;
        BarColor = Parent.MagazineBar.ForegroundGreen;
        Parent.MagazineBar.ForegroundGreen = 150;
    }

    public override void CustomActivity()
    {
        double progress = 100 * TimeManager.CurrentScreenSecondsSince(ReloadStartTime) / Parent.ReloadTimeSeconds;
        Parent.MagazineBar.ProgressPercentage = (float)progress;
    }

    public override GunController? EvaluateExitConditions()
    {
        if (TimeManager.CurrentScreenSecondsSince(ReloadStartTime) > Parent.ReloadTimeSeconds)
        {
            return Get<Ready>();
        }

        return null;
    }

    public override void BeforeDeactivate()
    {
        Parent.MagazineRemaining = Parent.MagazineSize;
        Parent.MagazineBar.ForegroundGreen = BarColor;
    }
}