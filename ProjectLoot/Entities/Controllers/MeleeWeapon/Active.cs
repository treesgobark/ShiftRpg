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
            Parent.Holder.InputEnabled = false;
            Hitbox = Parent.SpawnHitbox();
        }

        public override void CustomActivity()
        {
        }

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
            Parent.Holder.InputEnabled = true;
            Hitbox.Destroy();
        }

        public override void Uninitialize() { }
    }
}