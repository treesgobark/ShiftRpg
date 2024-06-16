using FlatRedBall;
using Microsoft.Xna.Framework;

namespace ProjectLoot.Entities
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
            if (TimeSinceLastDamage > 2.0)
            {
                Health.CurrentHealth = MaxHealth;
                Shatter.CurrentShatterDamage = 0;
                Shatter.CurrentShatterPercentage = 0;
                Weakness.CurrentWeaknessAmount = 0;
            }
        }

        private void CustomDestroy()
        {


        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {


        }
        
        public double TimeSinceLastDamage => TimeManager.CurrentScreenSecondsSince(Health.LastDamageTime);
    }
}
