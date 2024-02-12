using ANLG.Utilities.FlatRedBall.Controllers;
using FlatRedBall;
using ShiftRpg.Contracts;

namespace ShiftRpg.Controllers.Gun;

public abstract class GunController(IGun obj)
    : EntityController<IGun, GunController>(obj)
{
    private double StartTime { get; set; }
    protected double TimeInState => TimeManager.CurrentScreenSecondsSince(StartTime);

    public override void OnActivate()
    {
        StartTime = TimeManager.CurrentScreenTime;
    }
}