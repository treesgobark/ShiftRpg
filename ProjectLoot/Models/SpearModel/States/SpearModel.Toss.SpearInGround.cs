using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using ProjectLoot.Controllers;

namespace ProjectLoot.Models.SpearModel;

partial class SpearModel
{
    private class TossedSpearInGround : ParentedTimedState<Toss>
    {
        private readonly IReadonlyStateMachine _states;
        private static TimeSpan RecoveryDuration => TimeSpan.FromMilliseconds(960);
        
        private float NormalizedProgress => (float)(TimeInState / RecoveryDuration).Saturate();

        public TossedSpearInGround(IReadonlyStateMachine states, ITimeManager timeManager, Toss tossState)
            : base(timeManager, tossState)
        {
            _states = states;
        }
        
        protected override void AfterTimedStateActivate()
        {
            Parent.Hitbox.IsActive = false;
        }

        public override IState? EvaluateExitConditions()
        {
            // if (Parent.MeleeWeaponComponent.MeleeWeaponInputDevice.LightAttack.WasJustPressed)
            // {
            //     Parent.Hitbox?.Destroy();
            //     return _states.Get<Thrust>();
            // }
            
            if (Parent.MeleeWeaponComponent.MeleeWeaponInputDevice.HeavyAttack.WasJustPressed)
            {
                return _states.Get<TossRecall>();
            }
            
            if (NormalizedProgress >= 1)
            {
                return _states.Get<TossCleanup>();
            }

            return null;
        }

        protected override void AfterTimedStateActivity()
        {
            Parent.Hitbox.SpriteInstance.Alpha = 1f - MathF.Pow(NormalizedProgress, 3);
        }

        public override void BeforeDeactivate() { }
    }
}
