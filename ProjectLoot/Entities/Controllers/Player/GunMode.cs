using ANLG.Utilities.FlatRedBall.States;
using FlatRedBall.Input;

namespace ProjectLoot.Entities;

public partial class Player
{
    protected class GunMode : TimedState<Player>
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
            if (Parent.GameplayInputDevice.AimInMeleeRange)
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

        public override void Uninitialize() { }
    
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
