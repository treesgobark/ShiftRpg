using ANLG.Utilities.FlatRedBall.States;
using FlatRedBall.Input;

namespace ShiftRpg.Entities;

public partial class Player
{
    protected class Idle : TimedState<Player>
    {
        public Idle(Player parent, IStateMachine stateMachine) : base(parent, stateMachine) { }
        
        public override void Initialize() { }
    
        public override void OnActivate()
        {
            Parent.MeleeWeapon.Unequip();
            Parent.Gun.Unequip();
            Parent.AimThresholdCircle.Visible = false;
            Parent.AimThresholdCircle.Radius  = Parent.MeleeAimThreshold;
        }

        public override void CustomActivity()
        {
            SetRotation();
        }

        protected override void AfterTimedStateActivate() { }
    
        public override IState? EvaluateExitConditions()
        {
            if (!Parent.AimInMeleeRange)
            {
                return StateMachine.Get<GunMode>();
            }
            
            if (Parent.GameplayInputDevice.Attack.WasJustPressed)
            {
                return StateMachine.Get<MeleeMode>();
            }
            
            return null;
        }
    
        public override void BeforeDeactivate() { }

        public override void Uninitialize() { }
    
        private void SetRotation()
        {
            float? angle = Parent.GameplayInputDevice.Movement.GetAngle();
                
            if (angle is null)
            {
                Parent.RotationZ = Parent.LastMeleeRotation;
            }
            else
            {
                Parent.RotationZ = angle.Value;
                Parent.LastMeleeRotation = Parent.RotationZ;
            }
            
            Parent.ForceUpdateDependenciesDeep();
        }
    }
}
