using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using FlatRedBall.Input;
using ProjectLoot.Controllers;

namespace ProjectLoot.Entities;

public partial class DefaultRangedEnemy
{
    protected class GunMode : ParentedTimedState<DefaultRangedEnemy>
    {
        public GunMode(DefaultRangedEnemy parent, IReadonlyStateMachine stateMachine, ITimeManager timeManager)
            : base(parent, stateMachine, timeManager) { }

        public override void Initialize() { }

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

        public override void Uninitialize() { }
    
        private void SetRotation()
        {
            float? angle = Parent.EnemyInputDevice.Aim.GetAngle();
            
            if (angle is not null)
            {
                Parent.RotationZ = angle.Value;
            }
        
            Parent.ForceUpdateDependenciesDeep();
        }
    }
}
