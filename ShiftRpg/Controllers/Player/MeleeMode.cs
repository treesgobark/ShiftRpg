using FlatRedBall.Input;

namespace ShiftRpg.Controllers.Player;

public class MeleeMode(Entities.Player obj) : PlayerController(obj)
{
    public override void Initialize()
    {
    }

    public override void OnActivate()
    {
        Parent.MeleeWeapon.Equip();
        base.OnActivate();
    }

    public override void CustomActivity()
    {
        SetRotation();
        if (Parent.GameplayInputDevice.Attack.WasJustPressed)
        {
            Parent.MeleeWeapon.BeginAttack();
        }
        else if (Parent.GameplayInputDevice.Attack.WasJustReleased)
        {
            Parent.MeleeWeapon.EndAttack();
        }
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