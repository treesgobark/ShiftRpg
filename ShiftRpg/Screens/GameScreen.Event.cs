using System;
using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Specialized;
using FlatRedBall.Audio;
using FlatRedBall.Debugging;
using FlatRedBall.Entities;
using FlatRedBall.Math.Geometry;
using FlatRedBall.Screens;
using Microsoft.Xna.Framework;
using ShiftRpg.Entities;
using ShiftRpg.Screens;
namespace ShiftRpg.Screens
{
    public partial class GameScreen
    {
        void OnPlayerVsEnemyCollided (Entities.Player player, Entities.Enemy enemy) 
        {
            if (enemy is TargetDummy dummy)
            {
                dummy.CollideAgainstMove(player, 1, 0);
            }
            else
            {
                enemy.CollideAgainstMove(player, 1, 1);
            }
        }
    }
}
