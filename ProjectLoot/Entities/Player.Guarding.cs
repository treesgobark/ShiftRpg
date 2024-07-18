using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using FlatRedBall.Input;
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
            if (Parent.GameplayInputDevice.Dash.WasJustPressed && Parent.GameplayInputDevice.Movement.Magnitude > 0) return StateMachine.Get<Dashing>();

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
            SetRotation();
            Parent.HandleBobbing();
        }

        public override void BeforeDeactivate()
        {
            Parent.HealthComponent.DamageModifiers.Delete("guard");

            Parent.GuardSprite.Visible = false;

            Parent.CurrentMovement = TopDownValuesStatic[StoredMovementName];
        }

        public override void Uninitialize() { }
    
        private void SetRotation()
        {
            float? angle = Parent.GameplayInputDevice.Aim.GetAngle();

            if (angle is null)
            {
                return;
            }

            Rotation rotation = Rotation.FromRadians(angle.Value).Snap(8, true);

            Parent.RotationZ = rotation.NormalizedRadians;
            
            Rotation parentRotation = Rotation.FromRadians(Parent.RotationZ);
            int      sector         = parentRotation.GetSector(8, true);

            switch (sector)
            {
                case 0:
                    Parent.EyesSprite.Visible          = true;
                    Parent.EyesSprite.CurrentChainName = "EyesRight";
                    Parent.GunSprite.RelativeZ         = Parent.PlayerSprite.Z + 0.1f;
                    break;
                case 4:
                    Parent.EyesSprite.Visible          = true;
                    Parent.EyesSprite.CurrentChainName = "EyesLeft";
                    Parent.GunSprite.RelativeZ         = Parent.PlayerSprite.Z + 0.1f;
                    break;
                case 5:
                    Parent.EyesSprite.Visible          = true;
                    Parent.EyesSprite.CurrentChainName = "EyesDownLeft";
                    Parent.GunSprite.RelativeZ         = Parent.PlayerSprite.Z + 0.1f;
                    break;
                case 6:
                    Parent.EyesSprite.Visible          = true;
                    Parent.EyesSprite.CurrentChainName = "EyesDown";
                    Parent.GunSprite.RelativeZ         = Parent.PlayerSprite.Z + 0.1f;
                    break;
                case 7:
                    Parent.EyesSprite.Visible          = true;
                    Parent.EyesSprite.CurrentChainName = "EyesDownRight";
                    Parent.GunSprite.RelativeZ         = Parent.PlayerSprite.Z + 0.1f;
                    break;
                default:
                    Parent.EyesSprite.Visible  = false;
                    Parent.GunSprite.RelativeZ = Parent.PlayerSprite.Z - 0.1f;
                    break;
            }

            Parent.GameplayCenter.ForceUpdateDependenciesDeep();
        }
    }
}