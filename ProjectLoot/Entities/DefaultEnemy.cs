using FlatRedBall.Screens;
using ProjectLoot.InputDevices;
using ProjectLoot.Screens;

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
            var gameScreen = (GameScreen)ScreenManager.CurrentScreen;
            var target     = gameScreen.GetClosestPlayer(Position);
            if (InputDevice is EnemyInputDevice eInput)
            {
                eInput.SetTarget(target);
            }
        }

        private void CustomDestroy()
        {


        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {


        }
    }
}
