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
using ShiftRpg.Contracts;
using ShiftRpg.Factories;
using ShiftRpg.InputDevices;
using ShiftRpg.Screens;

namespace ShiftRpg.Entities
{
    public partial class Player
    {
        private IGun Gun { get; set; }
        private IGameplayInputDevice GameplayInputDevice { get; set; }
        
        private void CustomInitialize()
        {
            var gun = DefaultGunFactory.CreateNew();
            gun.AttachTo(this);
            gun.ParentRotationChangesRotation = true;
            Gun = gun;
            // InitializeTopDownInput(InputManager.Keyboard);
        }

        partial void CustomInitializeTopDownInput()
        {
            GameplayInputDevice = new GameplayInputDevice(InputDevice, this);
        }

        private void CustomActivity()
        {
            RotationZ = GameplayInputDevice.Aim.GetAngle() ?? 0;
            HandleGunInput();
        }

        private void CustomDestroy()
        {
        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {
        }

        private void HandleGunInput()
        {
            if (GameplayInputDevice.Attack.WasJustPressed)
            {
                Gun.BeginFire();
            }
            else if (GameplayInputDevice.Attack.WasJustReleased)
            {
                Gun.EndFire();
            }

            // if (GameplayInputDevice.Reload.WasJustPressed)
            // {
            //     Gun.Reload();
            // }
        }
    }
}
