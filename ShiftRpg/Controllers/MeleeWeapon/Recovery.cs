using System;
using Microsoft.Xna.Framework;

namespace ShiftRpg.Controllers.MeleeWeapon;

public class Recovery(Entities.MeleeWeapon obj) : MeleeWeaponController(obj)
{
    public override void Initialize()
    {
    }

    public override void OnActivate()
    {
        Parent.Owner.CircleInstance.Color = Color.Blue;
        Parent.Owner.InputEnabled         = false;
        base.OnActivate();
    }

    public override void CustomActivity()
    {
    }

    public override MeleeWeaponController? EvaluateExitConditions()
    {
        if (TimeInState > Parent.CurrentAttackData.RecoveryTime)
        {
            return Get<Idle>();
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