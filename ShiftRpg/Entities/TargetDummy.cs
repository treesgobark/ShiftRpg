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
        private float _currentHealth;

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
                CurrentHealth                                      = MaxHealth;
                HealthBarRuntimeInstance.MainBarProgressPercentage = CurrentHealthPercentage;
                CurrentShatterDamage = 0;
                HealthBarRuntimeInstance.ShatterBarProgressPercentage = ShatterSubProgressPercentage;
            }
        }

        private void CustomDestroy()
        {


        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {


        }

        public override float CurrentHealth
        {
            get => _currentHealth;
            set
            {
                _currentHealth                                     = (int)MathHelper.Clamp(value, -1, MaxHealth); 
                HealthBarRuntimeInstance.MainBarProgressPercentage = CurrentHealthPercentage;
            }
        }
        
        public double TimeSinceLastDamage => TimeManager.CurrentScreenSecondsSince(LastDamageTime);
    }
}
