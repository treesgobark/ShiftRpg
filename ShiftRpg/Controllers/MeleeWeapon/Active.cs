using Microsoft.Xna.Framework;

namespace ShiftRpg.Controllers.MeleeWeapon;

public class Active(Entities.MeleeWeapon obj) : MeleeWeaponController(obj)
{
    public override void Initialize()
    {
    }

    public override void OnActivate()
    {
        Parent.Owner.CircleInstance.Color = Color.Red;
        Parent.SecondsBetweenDamage       = Parent.CurrentAttackData.ActiveTime;
        Parent.PolygonInstance.Visible    = true;
        Parent.IsDamageDealingEnabled     = true;
        Parent.Owner.InputEnabled         = false;
        base.OnActivate();
    }

    public override void CustomActivity()
    {
    }

    public override MeleeWeaponController? EvaluateExitConditions()
    {
        if (TimeInState > Parent.CurrentAttackData.ActiveTime)
        {
            return Get<Recovery>();
        }

        return null;
    }

    public override void BeforeDeactivate()
    {
        Parent.PolygonInstance.Visible = false;
        Parent.IsDamageDealingEnabled  = false;
        Parent.Owner.InputEnabled      = true;
    }

    public override void BeginAttack() { }
    public override void EndAttack() { }
}