using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Graphics.Particle;
using FlatRedBall.Math.Geometry;
using Microsoft.Xna.Framework;
using ShiftRpg.Contracts;
using ShiftRpg.Effects;

namespace ShiftRpg.Entities
{
    public partial class TargetDummy
    {
        /// <summary>
        /// Initialization logic which is executed only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
        private void CustomInitialize()
        {


        }

        private void CustomActivity()
        {
            if (TimeSinceLastDamage > 1.5)
            {
                CurrentHealth                                    = MaxHealth;
                EnemyHealthBarRuntimeInstance.ProgressPercentage = CurrentHealthPercentage;
            }

        }

        private void CustomDestroy()
        {


        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {


        }

        public override void HandleEffects(IReadOnlyList<IEffect> effects)
        {
            foreach (var effect in effects)
            {
                if (RecentEffects.Any(t => t.EffectId == effect.EffectId))
                {
                    continue;
                }
                
                effect.HandleStandardDamage(this);
            }
        }

        public override void TakeDamage(int damage)
        {
            CurrentHealth                                    -= damage;
            EnemyHealthBarRuntimeInstance.ProgressPercentage =  CurrentHealthPercentage;
        }
    }
}
