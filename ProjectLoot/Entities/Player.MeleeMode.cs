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
            Parent.MeleeWeaponComponent.Equip(Parent.GameplayInputDevice.MeleeWeaponInputDevice);
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
            if (Parent.GameplayInputDevice.Dash.WasJustPressed)
            {
                return StateMachine.Get<Dashing>();
            }

            if (Parent.GameplayInputDevice.Guard.IsDown)
            {
                return StateMachine.Get<Guarding>();
            }

            if (!Parent.GameplayInputDevice.AimInMeleeRange && !Parent.GunComponent.IsEmpty)
            {
                return StateMachine.Get<GunMode>();
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
        }
    }
}
