using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using ANLG.Utilities.Core.StaticUtilities;
using ANLG.Utilities.FlatRedBall.Extensions;
using Microsoft.Xna.Framework;
using ProjectLoot.Controllers;

namespace ProjectLoot.Models.SpearModel;

partial class SpearModel
{
    private class TossWindup : ParentedTimedState<Toss>
    {
        private static TimeSpan MaxChargeDuration => TimeSpan.FromMilliseconds(480);
        private static TimeSpan MaxWindupDuration => TimeSpan.FromMilliseconds(960);

        private float NormalizedProgress => (float)(TimeInState / MaxWindupDuration).Saturate();
        private float ChargeProgress => (float)(TimeInState / MaxChargeDuration).Saturate();
        
        private bool IsCharged => ChargeProgress >= 1;

        private static float LateralOffset => 24;
        private static float PerpendicularOffset => -4;
        private static float WindupDistance => -16;

        private Vector3 CurrentWindupVector =>
            Vector3Extensions.FromRotationAndLength(Parent.AttackDirection, LateralOffset + ChargeProgress * WindupDistance)
            + Vector3Extensions.FromRotationAndLength(Parent.AttackDirection + Rotation.QuarterTurn, PerpendicularOffset);

        public TossWindup(IReadonlyStateMachine states, ITimeManager timeManager, Toss tossState)
            : base(states, timeManager, tossState) { }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate(IState? previousState)
        {
        }

        public override IState? EvaluateExitConditions()
        {
            if (NormalizedProgress >= 1)
            {
                return EmptyState.Instance;
            }
            
            if (!Parent.MeleeWeaponComponent.MeleeWeaponInputDevice.HeavyAttack.IsDown)
            {
                return IsCharged ? States.Get<TossActive>() : EmptyState.Instance;
            }

            return null;
        }

        protected override void AfterTimedStateActivity()
        {
            Parent.Hitbox.RelativeX = CurrentWindupVector.X;
            Parent.Hitbox.RelativeY = CurrentWindupVector.Y;
        }

        public override void BeforeDeactivate(IState? nextState)
        {
            if (nextState is TossActive)
            {
                Parent.ChargeProgress = ChargeProgress;
                return;
            }
            
            Parent.Hitbox?.Destroy();
        }

        public override void Uninitialize() { }
    }
}
