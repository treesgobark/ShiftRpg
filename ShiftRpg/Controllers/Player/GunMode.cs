using FlatRedBall.Input;

namespace ShiftRpg.Controllers.Player;

public class GunMode(Entities.Player obj) : PlayerController(obj)
{
    public override void Initialize()
    {
    }

    public override void OnActivate()
    {
        Parent.Gun.Equip(Parent.GunInputDevice);
        base.OnActivate();
    }

    public override void CustomActivity()
    {
        SetRotation();
    }

    public override PlayerController? EvaluateExitConditions()
    {
        if (Parent.AimInMeleeRange)
        {
            return Get<MeleeMode>();
        }

        // if (TimeInState > 3)
        // {
        //     return Get<Idle>();
        // }
        
        return null;
    }

    public override void BeforeDeactivate()
    {
        Parent.Gun.Unequip();
    }
    
    private void SetRotation()
    {
        float? angle = Parent.GameplayInputDevice.Aim.GetAngle();
            
        if (angle is not null)
        {
            Parent.RotationZ = angle.Value;
        }
        
        Parent.ForceUpdateDependenciesDeep();
    }
}