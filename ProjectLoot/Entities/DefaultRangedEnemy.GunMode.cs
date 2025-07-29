using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using FlatRedBall.Input;
using ProjectLoot.Controllers;

namespace ProjectLoot.Entities;

public partial class DefaultRangedEnemy
{
    protected class GunMode : ParentedTimedState<DefaultRangedEnemy>
    {
        public GunMode(DefaultRangedEnemy parent, IReadonlyStateMachine states, ITimeManager timeManager)
            : base(timeManager, parent) { }

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
            
            if (Parent.EnemyInputDevice.Modifier.WasJustPressed)
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
