using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using ProjectLoot.Contracts;
using ProjectLoot.Controllers;

namespace ProjectLoot.Entities;

partial class MeleeWeapon
{
    protected class Active : ParentedTimedState<MeleeWeapon>
    {
        public Active(MeleeWeapon parent, IReadonlyStateMachine stateMachine, ITimeManager timeManager)
            : base(parent, stateMachine, timeManager) { }

        protected MeleeHitbox Hitbox { get; set; }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate()
        {
            Hitbox = Parent.SpawnHitbox();
        }

        protected override void AfterTimedStateActivity() { }

        public override IState? EvaluateExitConditions()
        {
            if (TimeInState > Parent.CurrentAttackData.ActiveTimeSpan)
            {
                return StateMachine.Get<Recovery>();
            }

            return null;
        }

        public override void BeforeDeactivate()
        {
            Hitbox.Destroy();
        }

        public override void Uninitialize() { }
    }
}