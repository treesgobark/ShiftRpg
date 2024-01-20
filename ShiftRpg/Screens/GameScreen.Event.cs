using System;
using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Specialized;
using FlatRedBall.Audio;
using FlatRedBall.Debugging;
using FlatRedBall.Entities;
using FlatRedBall.Screens;
using Microsoft.Xna.Framework;
using ShiftRpg.Entities;
using ShiftRpg.Screens;
namespace ShiftRpg.Screens
{
    public partial class GameScreen
    {
        void OnEnemyVsBulletCollided (Entities.Enemy enemy, Entities.Bullet bullet)
        {
            if (!enemy.ShouldTakeDamage(bullet)) return;
            
            var dir = bullet.Velocity.NormalizedOrZero();
            enemy.Position += dir * 10;
        }
        
    }
}
