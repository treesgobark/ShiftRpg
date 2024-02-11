using System;
using System.Collections.Generic;
using System.Text;
using ANLG.Utilities.FlatRedBall.Constants;
using ANLG.Utilities.FlatRedBall.Extensions;
using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Graphics.Particle;
using FlatRedBall.Math.Geometry;
using Microsoft.Xna.Framework;
using RenderingLibrary.Graphics;

namespace ShiftRpg.Entities
{
    public partial class DamageNumber
    {
        /// <summary>
        /// Initialization logic which is executed only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
        private void CustomInitialize()
        {
            DamageNumberRuntimeInstance.TextInstance.OverrideTextRenderingPositionMode =
                TextRenderingPositionMode.FreeFloating;
            DamageNumberRuntimeInstance.TextInstance.FontScale = 0.75f;
            Velocity = 50f * Vector3.UnitX.RandomizeAngleBetween(MathConstants.EighthTurn, 3 * MathConstants.EighthTurn)
                .RandomizeMagnitudeBetween(0.5f, 1f);
        }

        private void CustomActivity()
        {
            DamageNumberRuntimeInstance.TextAlpha -= (int)(400f * TimeManager.SecondDifference);
            if (DamageNumberRuntimeInstance.TextAlpha <= 0)
            {
                Destroy();
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
