using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using FlatRedBall.Input;
using ProjectLoot.Controllers;

namespace ProjectLoot.Entities;

public partial class Player
{
    protected class Unarmed : ParentedTimedState<Player>
    {
        public Unarmed(Player parent, IReadonlyStateMachine stateMachine, ITimeManager timeManager)
            : base(stateMachine, timeManager, parent)
        {
        }

        public override void Initialize() { }

        protected override void AfterTimedStateActivate() { }

        protected override void AfterTimedStateActivity()
        {
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

            return (Parent.MeleeWeaponComponent.IsEmpty, Parent.GunComponent.IsEmpty,
                    Parent.GameplayInputDevice.AimInMeleeRange) switch
            {
                (true, true, _)  => null,
                (true, _, true) => null,
                (_, true, false) => null,
                (false, _, true) => StateMachine.Get<MeleeWeaponMode>(),
                (_, false, false) => StateMachine.Get<GunMode>(),
            };
        }

        public override void BeforeDeactivate() { }

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
        }
    }
}
