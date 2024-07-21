using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using FlatRedBall.Input;
using ProjectLoot.Controllers;

namespace ProjectLoot.Entities;

public partial class Player
{
    protected class MeleeWeaponMode : ParentedTimedState<Player>
    {
        public MeleeWeaponMode(Player parent, IReadonlyStateMachine states, ITimeManager timeManager)
            : base(states, timeManager, parent)
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
            Parent.HandleBobbing();
        }
    
        public override IState? EvaluateExitConditions()
        {
            if (Parent.GameplayInputDevice.Dash.WasJustPressed && Parent.GameplayInputDevice.Movement.Magnitude > 0)
            {
                return States.Get<Dashing>();
            }

            if (Parent.GameplayInputDevice.Guard.IsDown)
            {
                return States.Get<Guarding>();
            }
            
            if (!Parent.GameplayInputDevice.AimInMeleeRange)
            {
                return Parent.GunComponent.IsEmpty
                    ? States.Get<Unarmed>()
                    : States.Get<GunMode>();
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
                Parent.MeleeWeaponSprite.RelativeZ = Parent.PlayerSprite.Z - 0.1f;
            }
            else
            {
                Parent.MeleeWeaponSprite.RelativeZ = Parent.PlayerSprite.Z + 0.1f;
            }
            
            Rotation parentRotation = Rotation.FromRadians(Parent.RotationZ);
            int      sector         = parentRotation.GetSector(8, true);

            switch (sector)
            {
                case 0:
                    Parent.EyesSprite.Visible          = true;
                    Parent.EyesSprite.CurrentChainName = "EyesRight";
                    break;
                case 4:
                    Parent.EyesSprite.Visible          = true;
                    Parent.EyesSprite.CurrentChainName = "EyesLeft";
                    break;
                case 5:
                    Parent.EyesSprite.Visible          = true;
                    Parent.EyesSprite.CurrentChainName = "EyesDownLeft";
                    break;
                case 6:
                    Parent.EyesSprite.Visible          = true;
                    Parent.EyesSprite.CurrentChainName = "EyesDown";
                    break;
                case 7:
                    Parent.EyesSprite.Visible          = true;
                    Parent.EyesSprite.CurrentChainName = "EyesDownRight";
                    break;
                default:
                    Parent.EyesSprite.Visible = false;
                    break;
            }

            switch (Parent.XVelocity, Parent.YVelocity)
            {
                case (> 50, _):
                    Parent.PlayerSprite.CurrentChainName = "MoveRight";
                    Parent.PlayerSprite.AnimationSpeed   = 2f;
                    break;
                case (< -50, _):
                    Parent.PlayerSprite.CurrentChainName = "MoveLeft";
                    Parent.PlayerSprite.AnimationSpeed   = 2f;
                    break;
                case (_, > 50):
                    Parent.PlayerSprite.CurrentChainName = "MoveUp";
                    Parent.PlayerSprite.AnimationSpeed   = 2f;
                    break;
                case (_, < -50):
                    Parent.PlayerSprite.CurrentChainName = "MoveDown";
                    Parent.PlayerSprite.AnimationSpeed   = 2f;
                    break;
                default:
                    Parent.PlayerSprite.CurrentChainName = "Idle";
                    break;
            }

            Parent.GameplayCenter.ForceUpdateDependenciesDeep();
        }
    }
}
