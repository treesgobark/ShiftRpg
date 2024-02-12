using FlatRedBall.Input;

namespace ShiftRpg.Controllers.Player;

public class MeleeMode(Entities.Player obj) : PlayerController(obj)
{
    public override void Initialize()
    {
    }

    public override void OnActivate()
    {
        Parent.MeleeWeaponCache.IsActive = true;
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
        Parent.MeleeWeaponCache.IsActive = false;
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