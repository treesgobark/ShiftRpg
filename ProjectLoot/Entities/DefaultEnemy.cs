using ProjectLoot.InputDevices;

namespace ProjectLoot.Entities
{
    public partial class DefaultEnemy
    {
        /// <summary>
        /// Initialization logic which is executed only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
        private void CustomInitialize()
        {
            InitializeTopDownInput(new EnemyInputDevice(this));
        }

        private void CustomActivity()
        {
        }

        private void CustomDestroy()
        {


        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {


        }
    }
}
