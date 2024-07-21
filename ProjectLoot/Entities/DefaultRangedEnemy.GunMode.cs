using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using FlatRedBall.Input;
using ProjectLoot.Controllers;

namespace ProjectLoot.Entities;

public partial class DefaultRangedEnemy
{
    protected class GunMode : ParentedTimedState<DefaultRangedEnemy>
    {
        public GunMode(DefaultRangedEnemy parent, IReadonlyStateMachine states, ITimeManager timeManager)
            : base(states, timeManager, parent) { }

        public override void Initialize() { }

        protected override void AfterTimedStateActivate()
        {
            Parent.GunComponent.Equip();
        }

        protected override void AfterTimedStateActivity()
        {
            if (Parent.EnemyInputDevice.NextWeapon.WasJustPressed)
            {
                Parent.GunComponent.CycleToNextWeapon();
            }
            
            if (Parent.EnemyInputDevice.PreviousWeapon.WasJustPressed)
            {
                Parent.GunComponent.CycleToPreviousWeapon();
            }
            
            SetRotation();
        }

        public override IState? EvaluateExitConditions()
        {
            return null;
        }

        public override void BeforeDeactivate()
        {
            Parent.GunComponent.Unequip();
        }

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
