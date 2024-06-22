using ANLG.Utilities.FlatRedBall.States;
using FlatRedBall.Input;

namespace ProjectLoot.Entities;

public partial class DefaultMeleeEnemy
{
    protected class MeleeMode : TimedState<DefaultMeleeEnemy>
    {
        public MeleeMode(DefaultMeleeEnemy parent, IStateMachine stateMachine) : base(parent, stateMachine) { }
        
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
            
            float? angle = Parent.EnemyInputDevice.Movement.GetAngle();

            if (angle is not null)
            {
                Parent.RotationZ = angle.Value;
            }

            Parent.ForceUpdateDependenciesDeep();
        }
    }
}
