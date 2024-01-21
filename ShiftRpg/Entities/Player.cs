using ANLG.Utilities.FlatRedBall.Extensions;
using FlatRedBall;
using FlatRedBall.Entities;
using FlatRedBall.Graphics;
using FlatRedBall.Input;
using Microsoft.Xna.Framework;
using ShiftRpg.Contracts;
using ShiftRpg.Factories;
using ShiftRpg.InputDevices;

namespace ShiftRpg.Entities
{
    public partial class Player
    {
        private IGun Gun { get; set; }
        private IGameplayInputDevice GameplayInputDevice { get; set; }
        
        private void CustomInitialize()
        {
            InitializeGun();
            ReactToDamageReceived += OnReactToDamageReceived;
            var hudParent = gumAttachmentWrappers[0];
            hudParent.ParentRotationChangesRotation = false;
            InitializeTopDownInput(InputManager.Keyboard); // TODO: remove
        }

        private void InitializeGun()
        {
            var gun = DefaultGunFactory.CreateNew();
            gun.RelativeX = 10;
            gun.AttachTo(this);
            gun.ParentRotationChangesRotation = true;
            Gun = gun;
        }

        partial void CustomInitializeTopDownInput()
        {
            GameplayInputDevice = new GameplayInputDevice(InputDevice, this);
        }

        private void CustomActivity()
        {
            RotationZ = GameplayInputDevice.Aim.GetAngle() ?? 0;
            HandleInput();
        }

        private void CustomDestroy()
        {
            var gun = (IDestroyable)Gun;
            gun.Destroy();
        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {
        }

        private void OnReactToDamageReceived(decimal damage, IDamageArea area)
        {
            if (area is PositionedObject pObject)
            {
                Position += pObject.Velocity.NormalizedOrZero() * 30;
            }

            HealthBar.ProgressPercentage = (float)(100 * CurrentHealth / MaxHealth);
        }

        private void HandleInput()
        {
            if (GameplayInputDevice.Attack.WasJustPressed)
            {
                Gun.BeginFire();
            }
            else if (GameplayInputDevice.Attack.WasJustReleased)
            {
                Gun.EndFire();
            }

            if (GameplayInputDevice.Reload.WasJustPressed)
            {
                Gun.Reload();
            }

            if (GameplayInputDevice.Dash.WasJustPressed)
            {
                var dir = GameplayInputDevice.Movement.GetNormalizedPositionOrZero().ToVec3();
                if (dir != Vector3.Zero)
                {
                    Position += dir * DashDistance;
                }
            }
        }
    }
}
