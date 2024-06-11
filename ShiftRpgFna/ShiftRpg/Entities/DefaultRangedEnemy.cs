using FlatRedBall.Screens;
using ShiftRpg.InputDevices;
using ShiftRpg.Screens;

namespace ShiftRpg.Entities
{
    public partial class DefaultRangedEnemy
    {
        /// <summary>
        /// Initialization logic which is executed only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
        private void CustomInitialize()
        {
            InitializeTopDownInput(new RangedEnemyInputDevice(this, 100, 25));
        }

        private void CustomActivity()
        {
            var gameScreen = (GameScreen)ScreenManager.CurrentScreen;
            Player? target = gameScreen.GetClosestPlayer(Position);
            if (InputDevice is EnemyInputDevice eInput && target is not null)
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
