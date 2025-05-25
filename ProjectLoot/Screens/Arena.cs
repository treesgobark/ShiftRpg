using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ANLG.Utilities.Core.Extensions;
using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Debugging;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Gui;
using FlatRedBall.Math;
using FlatRedBall.Math.Geometry;
using FlatRedBall.Localization;
using Microsoft.Xna.Framework;

using ProjectLoot.Entities;
using ProjectLoot.Factories;


namespace ProjectLoot.Screens
{
    public partial class Arena
    {
        private void CustomInitialize()
        {
            SummonWaveButton.ButtonPushed += SpawnNextWave;
        }

        private void CustomActivity(bool firstTimeCalled)
        {
            
        }

        private void CustomDestroy()
        {
            
        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {
            
        }

        private void SpawnNextWave()
        {
            foreach (Spawner spawner in SpawnerList)
            {
                var dot = DotFactory.CreateNew(spawner.Position);
                dot.IsBig = Random.Shared.NextBool();
            }
        }
    }
}
