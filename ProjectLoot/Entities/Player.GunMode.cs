using System.Diagnostics;
using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using FlatRedBall.Input;
using ProjectLoot.Controllers;

namespace ProjectLoot.Entities;

public partial class Player
{
    protected class GunMode : ParentedTimedState<Player>
    {
        public GunMode(Player parent, IReadonlyStateMachine stateMachine, ITimeManager timeManager)
            : base(stateMachine, timeManager, parent) { }

        public override void Initialize() { }

        protected override void AfterTimedStateActivate()
        {
            Parent.GunComponent.Equip();
            Parent.TargetLineSprite.Visible = true;
            Parent.ReticleSprite.Visible = true;
        }

        protected override void AfterTimedStateActivity()
        {
            if (Parent.GameplayInputDevice.NextWeapon.WasJustPressed)
            {
                Parent.GunComponent.CycleToNextWeapon();
            }
            
            if (Parent.GameplayInputDevice.PreviousWeapon.WasJustPressed)
            {
                Parent.GunComponent.CycleToPreviousWeapon();
            }
            
            SetRotation();
        }

        public override IState? EvaluateExitConditions()
        {
            if (Parent.GameplayInputDevice.Dash.WasJustPressed && Parent.GameplayInputDevice.Movement.Magnitude > 0)
            {
                return StateMachine.Get<Dashing>();
            }

            if (Parent.GameplayInputDevice.Guard.IsDown)
            {
                return StateMachine.Get<Guarding>();
            }
            
            if (Parent.GameplayInputDevice.AimInMeleeRange)
            {
                return Parent.MeleeWeaponComponent.IsEmpty
                    ? StateMachine.Get<Unarmed>()
                    : StateMachine.Get<MeleeWeaponMode>();
            }
        
            return null;
        }

        public override void BeforeDeactivate()
        {
            Parent.GunComponent.Unequip();
            Parent.TargetLineSprite.Visible = false;
            Parent.ReticleSprite.Visible    = false;
        }

        public override void Uninitialize() { }
    
        private void SetRotation()
        {
            float? angle = Parent.GameplayInputDevice.Aim.GetAngle();

            if (angle is null)
            {
                return;
            }

            Parent.RotationZ = angle.Value;

            if (angle.Value is > 0 and < MathF.PI)
            {
                Parent.GunSprite.RelativeZ = Parent.Z - 0.5f;
            }
            else
            {
                Parent.GunSprite.RelativeZ = Parent.Z + 0.5f;
            }

            Parent.PlayerSprite.CurrentChainName = angle.Value switch
            {
                > 0 and <= MathF.PI             / 2                => "WalkForwardRight",
                > MathF.PI                      / 2 and < MathF.PI => "WalkForwardLeft",
                >= MathF.PI and <= 3 * MathF.PI / 2                => "WalkLeft",
                > 3 * MathF.PI       / 2 and <= 2 * MathF.PI or 0  => "WalkRight",
                _                                                  => throw new UnreachableException("")
            };

            Parent.GameplayCenter.ForceUpdateDependenciesDeep();
        }
    }
}
