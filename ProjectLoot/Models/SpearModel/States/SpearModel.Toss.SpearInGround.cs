using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using ANLG.Utilities.Core.StaticUtilities;
using ProjectLoot.Controllers;

namespace ProjectLoot.Models.SpearModel;

partial class SpearModel
{
    private class TossedSpearInGround : ParentedTimedState<Toss>
    {
        private static TimeSpan RecoveryDuration => TimeSpan.FromMilliseconds(960);
        
        private float NormalizedProgress => (float)(TimeInState / RecoveryDuration).Saturate();

        public TossedSpearInGround(IReadonlyStateMachine states, ITimeManager timeManager, Toss tossState)
            : base(states, timeManager, tossState) { }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate(IState? previousState)
        {
            Parent.Hitbox.IsActive = false;
        }

        public override IState? EvaluateExitConditions()
        {
            // if (Parent.MeleeWeaponComponent.MeleeWeaponInputDevice.LightAttack.WasJustPressed)
            // {
            //     Parent.Hitbox?.Destroy();
            //     return States.Get<Thrust>();
            // }
            
            if (Parent.MeleeWeaponComponent.MeleeWeaponInputDevice.HeavyAttack.WasJustPressed)
            {
                return States.Get<TossRecall>();
            }
            
            if (NormalizedProgress >= 1)
            {
                return EmptyState.Instance;
            }

            return null;
        }

        protected override void AfterTimedStateActivity()
        {
            Parent.Hitbox.SpriteInstance.Alpha = 1f - MathF.Pow(NormalizedProgress, 3);
        }

        public override void BeforeDeactivate(IState? nextState) { }

        public override void Uninitialize() { }
    }
}
