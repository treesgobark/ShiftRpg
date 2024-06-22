using ANLG.Utilities.FlatRedBall.States;
using ProjectLoot.Contracts;

namespace ProjectLoot.Entities;

partial class MeleeWeapon
{
    protected class Active : TimedState<IMeleeWeapon>
    {
        public Active(IMeleeWeapon parent, IReadonlyStateMachine stateMachine) : base(parent, stateMachine) { }

        protected MeleeHitbox Hitbox { get; set; }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate()
        {
            Hitbox = Parent.SpawnHitbox();
        }

        protected override void AfterTimedStateActivity() { }

        public override IState? EvaluateExitConditions()
        {
            if (TimeInState > Parent.CurrentAttackData.ActiveTime)
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