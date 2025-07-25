using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using FlatRedBall.Input;
using ProjectLoot.Controllers;

namespace ProjectLoot.Entities;

public partial class Player
{
    protected class GunMode : ParentedTimedState<Player>
    {
        public GunMode(Player parent, IReadonlyStateMachine states, ITimeManager timeManager)
            : base(states, timeManager, parent) { }

        public override void Initialize() { }

        protected override void AfterTimedStateActivate(IState? previousState)
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
            Parent.HandleBobbing();
        }

        public override IState? EvaluateExitConditions()
        {
            if (Parent.GameplayInputDevice.Dash.WasJustPressed && Parent.GameplayInputDevice.Movement.Magnitude > 0)
            {
                return States.Get<Dashing>();
            }

            if (Parent.GameplayInputDevice.Dash.WasJustPressed && Parent.GameplayInputDevice.Movement.Magnitude == 0)
            {
                return States.Get<Guarding>();
            }
            
            if (Parent.GameplayInputDevice.AimInMeleeRange)
            {
                return Parent.MeleeWeaponComponent.IsEmpty
                    ? States.Get<Unarmed>()
                    : States.Get<MeleeWeaponMode>();
            }
        
            return null;
        }

        public override void BeforeDeactivate(IState? nextState)
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
            
            Rotation parentRotation = Rotation.FromRadians(Parent.RotationZ);
            int      sector         = parentRotation.GetSector(8, true);

            switch (sector)
            {
                case 0:
                    Parent.EyesSprite.Visible          = true;
                    Parent.EyesSprite.CurrentChainName = "EyesRight";
                    Parent.GunSprite.RelativeZ         = Parent.PlayerSprite.Z + 0.1f;
                    break;
                case 4:
                    Parent.EyesSprite.Visible          = true;
                    Parent.EyesSprite.CurrentChainName = "EyesLeft";
                    Parent.GunSprite.RelativeZ         = Parent.PlayerSprite.Z + 0.1f;
                    break;
                case 5:
                    Parent.EyesSprite.Visible          = true;
                    Parent.EyesSprite.CurrentChainName = "EyesDownLeft";
                    Parent.GunSprite.RelativeZ         = Parent.PlayerSprite.Z + 0.1f;
                    break;
                case 6:
                    Parent.EyesSprite.Visible          = true;
                    Parent.EyesSprite.CurrentChainName = "EyesDown";
                    Parent.GunSprite.RelativeZ         = Parent.PlayerSprite.Z + 0.1f;
                    break;
                case 7:
                    Parent.EyesSprite.Visible          = true;
                    Parent.EyesSprite.CurrentChainName = "EyesDownRight";
                    Parent.GunSprite.RelativeZ         = Parent.PlayerSprite.Z + 0.1f;
                    break;
                default:
                    Parent.EyesSprite.Visible  = false;
                    Parent.GunSprite.RelativeZ = Parent.PlayerSprite.Z - 0.1f;
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
