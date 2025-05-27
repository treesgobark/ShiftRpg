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
        private static TimeSpan RecoveryDuration => TimeSpan.FromMilliseconds(960);
        
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
            // if (Parent.MeleeWeaponComponent.MeleeWeaponInputDevice.LightAttack.WasJustPressed)
            // {
            //     Parent.Hitbox?.Destroy();
            //     return States.Get<Thrust>();
            // }
            
            if (Parent.MeleeWeaponComponent.MeleeWeaponInputDevice.HeavyAttack.WasJustPressed)
            {
                return States.Get<TossRecall>();
            }
            
            if (!Parent.IsEquipped)
            {
                Parent.Hitbox?.Destroy();
                return States.Get<NotEquipped>();
            }

            if (NormalizedProgress >= 1)
            {
                Parent.Hitbox?.Destroy();
                return States.Get<Idle>();
            }

            return null;
        }

        protected override void AfterTimedStateActivity()
        {
            Parent.Hitbox.SpriteInstance.Alpha = 1f - MathF.Pow(NormalizedProgress, 3);
        }

        public override void BeforeDeactivate()
        {
        }

        public override void Uninitialize() { }
    }
}
