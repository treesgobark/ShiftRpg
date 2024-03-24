using ANLG.Utilities.FlatRedBall.States;
using FlatRedBall.Input;

namespace ShiftRpg.Entities;

public partial class Player
{
    public class MeleeMode : TimedState<Player>
    {
        public MeleeMode(Player parent, IStateMachine stateMachine) : base(parent, stateMachine) { }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate()
        {
            Parent.MeleeWeaponCache.IsActive = true;
        }

        public override void CustomActivity()
        {
            SetRotation();
        }
    
        public override IState? EvaluateExitConditions()
        {
            if (!Parent.AimInMeleeRange)
            {
                return StateMachine.Get<GunMode>();
            }
    
            if (TimeInState > 3)
            {
                return StateMachine.Get<Idle>();
            }
            
            return null;
        }
    
        public override void BeforeDeactivate()
        {
            Parent.MeleeWeaponCache.IsActive = false;
        }
    
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
            
            Parent.ForceUpdateDependenciesDeep();
        }
    }
}
