using FlatRedBall;
using Microsoft.Xna.Framework;
using ShiftRpg.Controllers.DefaultGun;

namespace ShiftRpg.Controllers.DefaultSword;

public class Active(Entities.DefaultSword parent) : DefaultSwordController(parent)
{
    private double StartTime { get; set; }
    private Color _color;
    
    public override void OnActivate()
    {
        StartTime                   = TimeManager.CurrentScreenTime;
        _color                      = parent.CircleInstance.Color;
        parent.CircleInstance.Color = Color.IndianRed;
        parent.IsDamageDealingEnabled             = true;
    }

    public override DefaultSwordController? EvaluateExitConditions()
    {
        if (TimeManager.CurrentScreenSecondsSince(StartTime) > 0.5)
        {
            return Get<DefaultSwordController>();
        }

        return null;
    }

    public override void BeforeDeactivate()
    {
        Parent.CircleInstance.Color   = _color;
        parent.IsDamageDealingEnabled = false;
    }

    public override void BeginAttack() { }
    public override void EndAttack() { }
}