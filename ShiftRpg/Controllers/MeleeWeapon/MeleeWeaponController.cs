using ANLG.Utilities.FlatRedBall.Controllers;
using FlatRedBall;
using ShiftRpg.Contracts;

namespace ShiftRpg.Controllers.MeleeWeapon;

public abstract class MeleeWeaponController(Entities.MeleeWeapon obj) : EntityController<Entities.MeleeWeapon, MeleeWeaponController>(obj)
{
    private double StartTime { get; set; }
    protected double TimeInState => TimeManager.CurrentScreenSecondsSince(StartTime);

    public override void OnActivate()
    {
        StartTime = TimeManager.CurrentScreenTime;
    }
    
    public abstract void BeginAttack();
    public abstract void EndAttack();
}