using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using FlatRedBall.Input;
using ProjectLoot.Controllers;

namespace ProjectLoot.Entities;

public partial class DefaultMeleeEnemy
{
    protected class MeleeMode : ParentedTimedState<DefaultMeleeEnemy>
    {
        public MeleeMode(DefaultMeleeEnemy parent, IReadonlyStateMachine states, ITimeManager timeManager)
            : base(timeManager, parent) { }
        
        protected override void AfterTimedStateActivate() { }

        protected override void AfterTimedStateActivity()
        {
            SetRotation();
        }
    
        public override IState? EvaluateExitConditions()
        {
            return null;
        }
    
        public override void BeforeDeactivate() { }

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
