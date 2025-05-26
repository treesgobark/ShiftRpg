using System.Collections.Generic;
using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using ANLG.Utilities.Core.StaticUtilities;
using FlatRedBall.Math.Geometry;
using Microsoft.Xna.Framework;
using ProjectLoot.Controllers;
using ProjectLoot.Effects;
using ProjectLoot.Effects.Base;
using ProjectLoot.Entities;
using ProjectLoot.Factories;

namespace ProjectLoot.Models.SpearModel;

partial class SpearModel
{
    private class TossedSpearInGround : ParentedTimedState<SpearModel>
    {
        private static TimeSpan RecoveryDuration => TimeSpan.FromMilliseconds(480);
        
        private float NormalizedProgress => (float)(TimeInState / RecoveryDuration).Saturate();

        public TossedSpearInGround(IReadonlyStateMachine states, ITimeManager timeManager, SpearModel weaponModel)
            : base(states, timeManager, weaponModel) { }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate()
        {
            Parent.Hitbox.IsActive = false;
        }

        public override IState? EvaluateExitConditions()
        {
            if (!Parent.IsEquipped)
            {
                return States.Get<NotEquipped>();
            }

            if (NormalizedProgress >= 1)
            {
                return States.Get<Idle>();
            }

            return null;
        }

        protected override void AfterTimedStateActivity()
        {
            Parent.Hitbox.SpriteInstance.Alpha = 1f - NormalizedProgress;
        }

        public override void BeforeDeactivate()
        {
            Parent.Hitbox?.Destroy();
        }

        public override void Uninitialize() { }
    }
}
