using FlatRedBall.Input;

namespace ShiftRpg.Controllers.Player;

public class MeleeMode(Entities.Player obj) : PlayerController(obj)
{
    public override void Initialize()
    {
    }

    public override void OnActivate()
    {
        Parent.MeleeWeapon.Equip(Parent.MeleeInputDevice);
        base.OnActivate();
    }

    public override void CustomActivity()
    {
        SetRotation();
    }

    public override PlayerController? EvaluateExitConditions()
    {
        if (!Parent.AimInMeleeRange)
        {
            return Get<GunMode>();
        }

        if (TimeInState > 3)
        {
            return Get<Idle>();
        }
        
        return null;
    }

    public override void BeforeDeactivate()
    {
        Parent.MeleeWeapon.Unequip();
    }

    private void SetRotation()
    {
        if (!Parent.InputEnabled)
        {
            return;
        }
        
        float? angle = Parent.GameplayInputDevice.Movement.GetAngle();
            
        if (angle is null)
        {
            Parent.RotationZ = Parent.LastMeleeRotation;
        }
        else
        {
            Parent.RotationZ         = angle.Value;
            Parent.LastMeleeRotation = Parent.RotationZ;
        }
        
        Parent.ForceUpdateDependenciesDeep();
    }
}