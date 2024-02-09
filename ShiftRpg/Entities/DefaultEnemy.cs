using System;
using System.Collections.Generic;
using System.Text;
using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Graphics.Particle;
using FlatRedBall.Math.Geometry;
using FlatRedBall.Screens;
using Microsoft.Xna.Framework;
using ShiftRpg.InputDevices;
using ShiftRpg.Screens;

namespace ShiftRpg.Entities
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
            InitializeTopDownInput(new EnemyInputDevice<Enemy>(this));
        }

        private void CustomActivity()
        {
            var gameScreen = (GameScreen)ScreenManager.CurrentScreen;
            var target     = gameScreen.GetClosestPlayer(Position);
            if (InputDevice is EnemyInputDevice<Enemy> eInput)
            {
                eInput.Target = target;
                InitializeTopDownInput(InputDevice);
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
