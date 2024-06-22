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
            Parent.Weapons.GunCache.IsActive = true;
        }

        protected override void AfterTimedStateActivity()
        {
            SetRotation();
        }

        public override IState? EvaluateExitConditions()
        {
            if (Parent.GameplayInputDevice.AimInMeleeRange)
            {
                return StateMachine.Get<MeleeMode>();
            }
        
            return null;
        }

        public override void BeforeDeactivate()
        {
            Parent.Weapons.GunCache.IsActive = false;
        }

        public override void Uninitialize() { }
    
        private void SetRotation()
        {
            float? angle = Parent.GameplayInputDevice.Aim.GetAngle();
            
            if (angle is not null)
            {
                Parent.RotationZ = angle.Value;
            }
        }
    }
}
