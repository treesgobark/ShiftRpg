using ANLG.Utilities.FlatRedBall.Controllers;
using ShiftRpg.Controllers.MeleeWeapon;

namespace ShiftRpg.Entities;

public partial class DefaultSword
{
    /// <summary>
    /// Initialization logic which is executed only one time for this Entity (unless the Entity is pooled).
    /// This method is called when the Entity is added to managers. Entities which are instantiated but not
    /// added to managers will not have this method called.
    /// </summary>
    private void CustomInitialize()
    {
        Controllers = new ControllerCollection<MeleeWeapon, MeleeWeaponController>();
        Controllers.Add(new Idle(this));
        Controllers.Add(new Startup(this));
        Controllers.Add(new Active(this));
        Controllers.Add(new Recovery(this));
        Controllers.InitializeStartingController<Idle>();
    }

    private void CustomActivity()
    {
        Controllers.DoCurrentControllerActivity();
    }

    private void CustomDestroy()
    {
    }

    private static void CustomLoadStaticContent(string contentManagerName)
    {
    }

    public override void BeginAttack() => Controllers.CurrentController.BeginAttack();
    public override void EndAttack() => Controllers.CurrentController.EndAttack();
}