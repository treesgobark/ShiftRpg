using ANLG.Utilities.FlatRedBall.States;
using Microsoft.Xna.Framework;
using ShiftRpg.Contracts;

namespace ShiftRpg.Entities;

partial class MeleeWeapon
{
    protected class Active : TimedState<MeleeWeapon>
    {
        public Active(MeleeWeapon parent, IStateMachine stateMachine) : base(parent, stateMachine) { }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate()
        {
            Parent.ShowHitbox(true);
            // Parent.Holder.SetPlayerColor(Color.Red);
            // Parent.IsDamageDealingEnabled     = true;
            // Parent.Holder.InputEnabled = false;
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
            Parent.ShowHitbox(false);
            // Parent.IsDamageDealingEnabled  = false;
            // Parent.Holder.InputEnabled = true;
            // Parent.TargetHitEffects   = EffectBundle.Empty;
        }

        public override void Uninitialize() { }
    }
}