using ANLG.Utilities.FlatRedBall.States;
using FlatRedBall.Input;

namespace ShiftRpg.Entities;

public partial class Player
{
    public class GunMode : TimedState<Player>
    {
        public GunMode(Player parent, IStateMachine stateMachine) : base(parent, stateMachine) { }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate()
        {
            Parent.GunCache.IsActive = true;
        }

        public override void CustomActivity()
        {
            SetRotation();
        }

        public override IState? EvaluateExitConditions()
        {
            if (Parent.AimInMeleeRange)
            {
                return StateMachine.Get<MeleeMode>();
            }

            // if (TimeInState > 3)
            // {
            //     return Get<Idle>();
            // }
        
            return null;
        }

        public override void BeforeDeactivate()
        {
            Parent.GunCache.IsActive = false;
        }
    
        private void SetRotation()
        {
            float? angle = Parent.GameplayInputDevice.Aim.GetAngle();
            
            if (angle is not null)
            {
                Parent.RotationZ = angle.Value;
            }
        
            Parent.ForceUpdateDependenciesDeep();
        }
    }
}
