using ANLG.Utilities.FlatRedBall.Controllers;
using FlatRedBall;
using FlatRedBall.Debugging;
using Microsoft.Xna.Framework;
using ShiftRpg.Factories;

namespace ShiftRpg.Controllers.DefaultSword;

public class DefaultSwordController(Entities.DefaultSword parent)
    : EntityController<Entities.DefaultSword, DefaultSwordController>(parent)
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
        NextState = Get<Active>();
    }

    public virtual void EndAttack()
    {
    }
}