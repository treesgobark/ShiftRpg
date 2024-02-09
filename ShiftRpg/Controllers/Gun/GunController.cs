using ANLG.Utilities.FlatRedBall.Controllers;
using FlatRedBall;

namespace ShiftRpg.Controllers.Gun;

public abstract class GunController(Entities.Gun obj)
    : EntityController<Entities.Gun, GunController>(obj)
{
    private double StartTime { get; set; }
    protected double TimeInState => TimeManager.CurrentScreenSecondsSince(StartTime);

    public override void OnActivate()
    {
        StartTime = TimeManager.CurrentScreenTime;
    }
}