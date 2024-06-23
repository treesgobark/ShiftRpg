using ANLG.Utilities.FlatRedBall.States;
using ProjectLoot.Contracts;

namespace ProjectLoot.Entities;

public partial class Player
{
    protected class Guarding : TimedState<Player>
    {
        private string StoredMovementName { get; set; }
        
        public Guarding(Player parent, IReadonlyStateMachine stateMachine) : base(parent, stateMachine)
        {
        }

        public override void Initialize() { }

        protected override void AfterTimedStateActivate()
        {
            Parent.Health.DamageModifiers.Upsert("guard", new StatModifier<float>(
                _ => true,
                _ => 0.2f,
                ModifierCategory.Multiplicative));

            Parent.GuardSprite.Visible = true;

            StoredMovementName = Parent.CurrentMovementName;
            Parent.CurrentMovement = TopDownValuesStatic[DataTypes.TopDownValues.Guarding];
        }

        public override IState? EvaluateExitConditions()
        {
            if (Parent.GameplayInputDevice.Guard.IsDown) { return null; }

            return Parent.GameplayInputDevice.AimInMeleeRange
                ? StateMachine.Get<MeleeMode>()
                : StateMachine.Get<GunMode>();
        }

        protected override void AfterTimedStateActivity() { }

        public override void BeforeDeactivate()
        {
            Parent.Health.DamageModifiers.Delete("guard");

            Parent.GuardSprite.Visible = false;

            Parent.CurrentMovement = TopDownValuesStatic[StoredMovementName];
        }

        public override void Uninitialize() { }
    }
}