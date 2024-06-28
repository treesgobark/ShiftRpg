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
            : base(parent, stateMachine, timeManager)
        {
        }

        public override void Initialize() { }

        protected override void AfterTimedStateActivate() { }

        protected override void AfterTimedStateActivity()
        {
            if (Parent.GameplayInputDevice.NextWeapon.WasJustPressed)
            {
                Parent.MeleeWeapon.Cache.CycleToNextWeapon();
            }
            
            if (Parent.GameplayInputDevice.PreviousWeapon.WasJustPressed)
            {
                Parent.MeleeWeapon.Cache.CycleToPreviousWeapon();
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

            return (Parent.MeleeWeapon.Cache.Count, Parent.Gun.Weapons.Count,
                    Parent.GameplayInputDevice.AimInMeleeRange) switch
            {
                (> 0, 0, _)       => StateMachine.Get<MeleeWeaponMode>(),
                (0, > 0, _)       => StateMachine.Get<GunMode>(),
                (> 0, > 0, true)  => StateMachine.Get<MeleeWeaponMode>(),
                (> 0, > 0, false) => StateMachine.Get<GunMode>(),
                _                 => null
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
