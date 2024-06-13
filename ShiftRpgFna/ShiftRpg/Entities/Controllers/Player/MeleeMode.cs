using ANLG.Utilities.FlatRedBall.States;
using FlatRedBall.Input;

namespace ShiftRpg.Entities;

public partial class Player
{
    protected class MeleeMode : TimedState<Player>
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
            if (!Parent.GameplayInputDevice.AimInMeleeRange)
            {
                return StateMachine.Get<GunMode>();
            }
            
            return null;
        }
    
        public override void BeforeDeactivate()
        {
            Parent.MeleeWeaponCache.IsActive = false;
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
            
            Parent.ForceUpdateDependenciesDeep();
        }
    }
}
