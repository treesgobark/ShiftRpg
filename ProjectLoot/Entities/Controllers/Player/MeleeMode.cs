using ANLG.Utilities.FlatRedBall.States;
using FlatRedBall.Input;

namespace ProjectLoot.Entities;

public partial class Player
{
    protected class MeleeMode : TimedState<Player>
    {
        public MeleeMode(Player parent, IStateMachine stateMachine) : base(parent, stateMachine) { }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate()
        {
            Parent.Weapons.MeleeWeaponCache.IsActive = true;
        }

        protected override void AfterTimedStateActivity()
        {
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

            if (!Parent.GameplayInputDevice.AimInMeleeRange)
            {
                return StateMachine.Get<GunMode>();
            }
            
            return null;
        }
    
        public override void BeforeDeactivate()
        {
            Parent.Weapons.MeleeWeaponCache.IsActive = false;
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
