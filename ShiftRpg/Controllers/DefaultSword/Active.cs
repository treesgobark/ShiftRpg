using FlatRedBall;
using Microsoft.Xna.Framework;
using ShiftRpg.Controllers.DefaultGun;

namespace ShiftRpg.Controllers.DefaultSword;

public class Active(Entities.DefaultSword obj) : DefaultSwordController(obj)
{
    private double StartTime { get; set; }
    private Color _color;
    
    public override void OnActivate()
    {
        StartTime                     = TimeManager.CurrentScreenTime;
        _color                        = Parent.PolygonInstance.Color;
        Parent.PolygonInstance.Color   = Color.IndianRed;
        Parent.IsDamageDealingEnabled = true;
    }

    public override DefaultSwordController? EvaluateExitConditions()
    {
        if (TimeManager.CurrentScreenSecondsSince(StartTime) > Parent.SwingActiveTime)
        {
            return Get<DefaultSwordController>();
        }

        return null;
    }

    public override void BeforeDeactivate()
    {
        Parent.PolygonInstance.Color   = _color;
        Parent.IsDamageDealingEnabled = false;
    }

    public override void BeginAttack() { }
    public override void EndAttack() { }
}