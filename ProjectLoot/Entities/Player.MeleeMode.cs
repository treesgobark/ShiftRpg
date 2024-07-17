using System.Diagnostics;
using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using FlatRedBall.Input;
using ProjectLoot.Controllers;

namespace ProjectLoot.Entities;

public partial class Player
{
    protected class MeleeWeaponMode : ParentedTimedState<Player>
    {
        public MeleeWeaponMode(Player parent, IReadonlyStateMachine stateMachine, ITimeManager timeManager)
            : base(stateMachine, timeManager, parent)
        {
        }

        public override void Initialize() { }

        protected override void AfterTimedStateActivate()
        {
            Parent.MeleeWeaponComponent.Equip();
        }

        protected override void AfterTimedStateActivity()
        {
            if (Parent.GameplayInputDevice.NextWeapon.WasJustPressed)
            {
                Parent.MeleeWeaponComponent.CycleToNextWeapon();
            }
            
            if (Parent.GameplayInputDevice.PreviousWeapon.WasJustPressed)
            {
                Parent.MeleeWeaponComponent.CycleToPreviousWeapon();
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
            
            if (!Parent.GameplayInputDevice.AimInMeleeRange)
            {
                return Parent.GunComponent.IsEmpty
                    ? StateMachine.Get<Unarmed>()
                    : StateMachine.Get<GunMode>();
            }
            
            return null;
        }

        public override void BeforeDeactivate()
        {
            Parent.MeleeWeaponComponent.Unequip();
        }

        public override void Uninitialize() { }
    
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

            if (Parent.RotationZ is > 0 and <= MathF.PI)
            {
                Parent.MeleeWeaponSprite.RelativeZ = Parent.Z - 0.5f;
            }
            else
            {
                Parent.MeleeWeaponSprite.RelativeZ = Parent.Z + 0.5f;
            }

            Parent.PlayerSprite.CurrentChainName = Parent.RotationZ switch
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
