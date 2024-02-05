using System;
using Microsoft.Xna.Framework;

namespace ShiftRpg.Controllers.MeleeWeapon;

public class Startup(Entities.MeleeWeapon obj) : MeleeWeaponController(obj)
{
    public override void Initialize()
    {
    }

    public override void OnActivate()
    {
        Parent.Owner.CircleInstance.Color = Color.Yellow;
        Parent.Owner.InputEnabled         = false;
        base.OnActivate();
    }

    public override void CustomActivity()
    {
    }

    public override MeleeWeaponController? EvaluateExitConditions()
    {
        if (TimeInState > Parent.CurrentAttackData.StartupTime)
        {
            return Get<Active>();
        }

        return null;
    }

    public override void BeforeDeactivate()
    {
        Parent.Owner.InputEnabled = true;
    }

    public override void BeginAttack()
    {
    }

    public override void EndAttack()
    {
    }
}