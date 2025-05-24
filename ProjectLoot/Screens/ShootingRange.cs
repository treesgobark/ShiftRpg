using FlatRedBall.Input;
using Microsoft.Xna.Framework.Input;
using ProjectLoot.Factories;

namespace ProjectLoot.Screens
{
    public partial class ShootingRange
    {

        void CustomInitialize()
        {


        }

        void CustomActivity(bool firstTimeCalled)
        {
            if (InputManager.Keyboard.KeyPushed(Keys.D1))
            {
                DotFactory.CreateNew(216, -440);
            }
            if (InputManager.Keyboard.KeyPushed(Keys.D5))
            {
                for (int i = 0; i < 5; i++)
                {
                    DotFactory.CreateNew(216, -440);
                }
            }
        }

        void CustomDestroy()
        {


        }

        static void CustomLoadStaticContent(string contentManagerName)
        {


        }

    }
}
