using ANLG.Utilities.Core;
using ANLG.Utilities.FlatRedBall.Extensions;
using ANLG.Utilities.States;
using Microsoft.Xna.Framework;
using ProjectLoot.Controllers;

namespace ProjectLoot.Models.SpearModel;

partial class SpearModel
{
    private class TossWindup : DurationState<Toss>
    {
        private readonly IReadonlyStateMachine _states;
        private static TimeSpan MaxChargeDuration => TimeSpan.FromMilliseconds(480);
        public override TimeSpan Duration => TimeSpan.FromMilliseconds(960);
        
        private float ChargeProgress => (float)(TimeInState / MaxChargeDuration).Saturate();
        private bool HasChargeCompleted => ChargeProgress >= 1;

        private static float LateralOffset => 24;
        private static float PerpendicularOffset => -4;
        private static float WindupDistance => -16;

        private Vector3 CurrentWindupVector =>
            Vector3Extensions.FromRotationAndLength(Parent.AttackDirection, LateralOffset + ChargeProgress * WindupDistance)
            + Vector3Extensions.FromRotationAndLength(Parent.AttackDirection + Rotation.QuarterTurn, PerpendicularOffset);

        public TossWindup(IReadonlyStateMachine states, ITimeManager timeManager, Toss tossState)
            : base(timeManager, tossState)
        {
            _states = states;
        }
        
        protected override void AfterTimedStateActivate()
        {
        }

        public override IState? EvaluateExitConditions()
        {
            if (HasDurationCompleted)
            {
                return _states.Get<TossCleanup>();
            }
            
            if (!Parent.MeleeWeaponComponent.MeleeWeaponInputDevice.HeavyAttack.IsDown)
            {
                return HasChargeCompleted ? _states.Get<TossActive>() : _states.Get<TossCleanup>();
            }

            return null;
        }

        protected override void AfterTimedStateActivity()
        {
            Parent.Hitbox.RelativeX = CurrentWindupVector.X;
            Parent.Hitbox.RelativeY = CurrentWindupVector.Y;
        }

        public override void BeforeDeactivate()
        {
            Parent.ChargeProgress = ChargeProgress;
        }
    }
}
