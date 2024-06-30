using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using ProjectLoot.Contracts;
using ProjectLoot.Controllers;

namespace ProjectLoot.Entities;

public partial class Player
{
    protected class Guarding : ParentedTimedState<Player>
    {
        public Guarding(Player parent, IReadonlyStateMachine stateMachine, ITimeManager timeManager)
            : base(stateMachine, timeManager, parent)
        {
        }

        private string StoredMovementName { get; set; } = null!;

        public override void Initialize()
        {
        }

        protected override void AfterTimedStateActivate()
        {
            Parent.HealthComponent.DamageModifiers.Upsert("guard", new StatModifier<float>(
                                                     _ => true,
                                                     _ => 0.2f,
                                                     ModifierCategory.Multiplicative));

            Parent.GuardSprite.Visible = true;

            StoredMovementName     = Parent.CurrentMovementName;
            Parent.CurrentMovement = TopDownValuesStatic[DataTypes.TopDownValues.Guarding];
        }

        public override IState? EvaluateExitConditions()
        {
            if (Parent.GameplayInputDevice.Dash.WasJustPressed) return StateMachine.Get<Dashing>();

            if (Parent.GameplayInputDevice.Guard.IsDown) return null;

            return (Parent.MeleeWeaponComponent.IsEmpty, Parent.GunComponent.IsEmpty,
                    Parent.GameplayInputDevice.AimInMeleeRange) switch
            {
                (false, true, _)      => StateMachine.Get<MeleeWeaponMode>(),
                (true, false, _)      => StateMachine.Get<GunMode>(),
                (false, false, true)  => StateMachine.Get<MeleeWeaponMode>(),
                (false, false, false) => StateMachine.Get<GunMode>(),
                _                     => StateMachine.Get<Unarmed>()
            };
        }

        protected override void AfterTimedStateActivity()
        {
        }

        public override void BeforeDeactivate()
        {
            Parent.HealthComponent.DamageModifiers.Delete("guard");

            Parent.GuardSprite.Visible = false;

            Parent.CurrentMovement = TopDownValuesStatic[StoredMovementName];
        }

        public override void Uninitialize()
        {
        }
    }
}