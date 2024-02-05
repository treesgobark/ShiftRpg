using ANLG.Utilities.FlatRedBall.Extensions;
using Microsoft.Xna.Framework;
using ShiftRpg.DataTypes;

namespace ShiftRpg.Controllers.MeleeWeapon;

public class Idle(Entities.MeleeWeapon obj) : MeleeWeaponController(obj)
{
    protected MeleeWeaponController? NextState { get; set; }
    
    public override void Initialize() { }

    public override void OnActivate()
    {
        if (Parent.Owner is not null)
        {
            Parent.Owner.CircleInstance.Color = Color.Green;
        }
        
        base.OnActivate();
    }

    public override void CustomActivity() { }

    public override MeleeWeaponController? EvaluateExitConditions()
    {
        return NextState;
    }

    public override void BeforeDeactivate()
    {
        NextState = null;
    }

    public override void BeginAttack()
    {
        Parent.Owner.Velocity += Parent.AttackForwardVelocity * Vector2ExtensionMethods.FromAngle(Parent.Owner.RotationZ).ToVec3();
        Parent.CurrentAttackData = GlobalContent.AttackData[AttackData.DefaultSwordSlash];
        NextState = Get<Startup>();
    }

    public override void EndAttack()
    {
    }
}