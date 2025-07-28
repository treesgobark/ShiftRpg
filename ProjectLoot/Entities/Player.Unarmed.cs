using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using FlatRedBall.Input;
using ProjectLoot.Controllers;

namespace ProjectLoot.Entities;

public partial class Player
{
    protected class Unarmed : ParentedTimedState<Player>
    {
        private readonly IReadonlyStateMachine _states;

        public Unarmed(Player parent, IReadonlyStateMachine states, ITimeManager timeManager)
            : base(timeManager, parent)
        {
            _states = states;
        }

        protected override void AfterTimedStateActivate() { }

        protected override void AfterTimedStateActivity()
        {
            SetRotation();
            Parent.HandleBobbing();
        }

        public override IState? EvaluateExitConditions()
        {
            if (Parent.GameplayInputDevice.Dash.WasJustPressed && Parent.GameplayInputDevice.Movement.Magnitude > 0)
            {
                return _states.Get<Dashing>();
            }

            if (Parent.GameplayInputDevice.Dash.WasJustPressed && Parent.GameplayInputDevice.Movement.Magnitude == 0)
            {
                return _states.Get<Guarding>();
            }

            return (Parent.MeleeWeaponComponent.IsEmpty, Parent.GunComponent.IsEmpty,
                    Parent.GameplayInputDevice.AimInMeleeRange) switch
            {
                (true, true, _)  => null,
                (true, _, true) => null,
                (_, true, false) => null,
                (false, _, true) => _states.Get<MeleeWeaponMode>(),
                (_, false, false) => _states.Get<GunMode>(),
            };
        }

        public override void BeforeDeactivate() { }

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
            
            Rotation parentRotation = Rotation.FromRadians(Parent.RotationZ);
            int      sector         = parentRotation.GetSector(8, true);

            switch (sector)
            {
                case 0:
                    Parent.EyesSprite.Visible = true;
                    Parent.EyesSprite.CurrentChainName = "EyesRight";
                    break;
                case 4:
                    Parent.EyesSprite.Visible = true;
                    Parent.EyesSprite.CurrentChainName = "EyesLeft";
                    break;
                case 5:
                    Parent.EyesSprite.Visible = true;
                    Parent.EyesSprite.CurrentChainName = "EyesDownLeft";
                    break;
                case 6:
                    Parent.EyesSprite.Visible = true;
                    Parent.EyesSprite.CurrentChainName = "EyesDown";
                    break;
                case 7:
                    Parent.EyesSprite.Visible = true;
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
