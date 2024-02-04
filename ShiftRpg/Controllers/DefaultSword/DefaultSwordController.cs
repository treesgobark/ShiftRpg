using ANLG.Utilities.FlatRedBall.Controllers;
using ANLG.Utilities.FlatRedBall.Extensions;
using FlatRedBall;
using FlatRedBall.Debugging;
using Microsoft.Xna.Framework;
using ShiftRpg.Factories;

namespace ShiftRpg.Controllers.DefaultSword;

public class DefaultSwordController(Entities.DefaultSword obj)
    : EntityController<Entities.DefaultSword, DefaultSwordController>(obj)
{
    protected DefaultSwordController? NextState { get; set; }
    
    public override void Initialize() { }

    public override void OnActivate()
    {
    }

    public override void CustomActivity() { }

    public override DefaultSwordController? EvaluateExitConditions()
    {
        if (NextState is not null)
        {
            return NextState;
        }

        return null;
    }

    public override void BeforeDeactivate()
    {
        NextState = null;
    }

    public virtual void BeginAttack()
    {
        Parent.Owner.Velocity += Parent.AttackForwardVelocity * Vector2ExtensionMethods.FromAngle(Parent.Owner.RotationZ).ToVec3();
        NextState             =  Get<Active>();
    }

    public virtual void EndAttack()
    {
    }
}