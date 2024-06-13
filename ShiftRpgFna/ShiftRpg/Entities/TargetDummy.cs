using FlatRedBall;
using Microsoft.Xna.Framework;

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
                CurrentHealth = MaxHealth;
                CurrentShatterDamage = 0;
                CurrentWeaknessAmount = 0;
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
