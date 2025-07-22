using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using FlatRedBall.Input;
using ProjectLoot.Contracts;
using ProjectLoot.Controllers;

namespace ProjectLoot.Entities;

public partial class Player
{
    protected class Guarding : ParentedTimedState<Player>
    {
        public Guarding(Player parent, IReadonlyStateMachine states, ITimeManager timeManager)
            : base(states, timeManager, parent)
        {
        }

        private string StoredMovementName { get; set; } = null!;

        public override void Initialize()
        {
        }

        protected override void AfterTimedStateActivate(IState? previousState)
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
            if (Parent.GameplayInputDevice.Dash.IsDown) return null;

            return (Parent.MeleeWeaponComponent.IsEmpty, Parent.GunComponent.IsEmpty,
                    Parent.GameplayInputDevice.AimInMeleeRange) switch
            {
                (false, true, _)      => States.Get<MeleeWeaponMode>(),
                (true, false, _)      => States.Get<GunMode>(),
                (false, false, true)  => States.Get<MeleeWeaponMode>(),
                (false, false, false) => States.Get<GunMode>(),
                _                     => States.Get<Unarmed>()
            };
        }

        protected override void AfterTimedStateActivity()
        {
            SetRotation();
            Parent.HandleBobbing();
        }

        public override void BeforeDeactivate(IState? nextState)
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