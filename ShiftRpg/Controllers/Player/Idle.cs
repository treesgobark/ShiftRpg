using FlatRedBall.Input;

namespace ShiftRpg.Controllers.Player;

public class Idle(Entities.Player obj) : PlayerController(obj)
{
    public override void Initialize()
    {
    }

    public override void OnActivate()
    {
        Parent.MeleeWeapon.Unequip();
        Parent.Gun.Unequip();
        Parent.AimThresholdCircle.Visible = false;
        Parent.AimThresholdCircle.Radius  = Parent.MeleeAimThreshold;
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
        
        if (Parent.GameplayInputDevice.Attack.WasJustPressed)
        {
            return Get<MeleeMode>();
        }
        
        return null;
    }

    public override void BeforeDeactivate()
    {
    }

    private void SetRotation()
    {
        float? angle = Parent.GameplayInputDevice.Movement.GetAngle();
            
        if (angle is null)
        {
            Parent.RotationZ = Parent.LastMeleeRotation;
        }
        else
        {
            Parent.RotationZ = angle.Value;
            Parent.LastMeleeRotation = Parent.RotationZ;
        }
        
        Parent.ForceUpdateDependenciesDeep();
    }
}