using ANLG.Utilities.FlatRedBall.Controllers;
using FlatRedBall;
using FlatRedBall.Debugging;
using Microsoft.Xna.Framework;
using ShiftRpg.Factories;

namespace ShiftRpg.Controllers.DefaultGun;

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