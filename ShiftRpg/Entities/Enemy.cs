using System;
using System.Collections.Generic;
using System.Text;
using ANLG.Utilities.FlatRedBall.Extensions;
using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Debugging;
using FlatRedBall.Entities;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Graphics.Particle;
using FlatRedBall.Math.Geometry;
using FlatRedBall.Screens;
using Microsoft.Xna.Framework;
using ShiftRpg.InputDevices;
using ShiftRpg.Screens;

namespace ShiftRpg.Entities
{
    public partial class Enemy
    {
        /// <summary>
        /// Initialization logic which is executed only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
        private void CustomInitialize()
        {
            ReactToDamageReceived += OnReactToDamageReceived;
            var hudParent = gumAttachmentWrappers[0];
            hudParent.ParentRotationChangesRotation = false;
            InitializeTopDownInput(new EnemyInputDevice<Enemy>(this));
        }

        private void OnReactToDamageReceived(decimal damage, IDamageArea area)
        {
            if (area is Bullet bullet)
            {
                Velocity += bullet.Velocity.NormalizedOrZero() * bullet.KnockbackVelocity / KnockbackResistance;
            }

            if (area is MeleeWeapon melee)
            {
                Velocity += melee.Owner.GetForwardVector3() * melee.KnockbackVelocity / KnockbackResistance;
            }

            EnemyHealthBarRuntimeInstance.ProgressPercentage = (float)(100 * CurrentHealth / MaxHealth);
        }

        private void CustomActivity()
        {
            var gameScreen = (GameScreen)ScreenManager.CurrentScreen;
            var target = gameScreen.GetClosestPlayer(Position);
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
