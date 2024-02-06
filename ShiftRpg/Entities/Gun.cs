using System;
using System.Collections.Generic;
using ANLG.Utilities.FlatRedBall.Controllers;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using FlatRedBall.Debugging;
using FlatRedBall.Input;
using ShiftRpg.Contracts;
using ShiftRpg.Controllers.DefaultGun;
using ShiftRpg.DataTypes;
using ShiftRpg.InputDevices;

namespace ShiftRpg.Entities
{
    public abstract partial class Gun : IGun, IHasControllers<Gun, GunController>
    {
        public GunData CurrentGunData { get; set; }
        protected readonly CyclableList<string> GunList = new(GunData.OrderedList);
        
        private int _magazineRemaining;

        public int MagazineRemaining
        {
            get => _magazineRemaining;
            set
            {
                _magazineRemaining = value;
                MagazineBar.ProgressPercentage = 100 * _magazineRemaining / (float)MagazineSize;
            }
        }

        /// <summary>
        /// Initialization logic which is executed only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
        private void CustomInitialize()
        {
            MagazineRemaining = MagazineSize;
            
            var hudParent = gumAttachmentWrappers[0];
            hudParent.ParentRotationChangesRotation = false;

            CurrentGunData = GlobalContent.GunData[GunList.CycleToNextItem()];
        }

        private void CustomActivity()
        {
            if (InputManager.Mouse.ScrollWheelChange > 0)
            {
                CurrentGunData = GlobalContent.GunData[GunList.CycleToNextItem()];
                Debugger.CommandLineWrite($"Switched to {CurrentGunData.GunName}");
            }
            else if (InputManager.Mouse.ScrollWheelChange < 0)
            {
                CurrentGunData = GlobalContent.GunData[GunList.CycleToPreviousItem()];
                Debugger.CommandLineWrite($"Switched to {CurrentGunData.GunName}");
            }
        }

        private void CustomDestroy() { }
        private static void CustomLoadStaticContent(string contentManagerName) { }
        
        // Implement IHasControllers
        
        public ControllerCollection<Gun, GunController> Controllers { get; protected set; }
        
        // Implement IGun

        public Action<IReadOnlyList<object>> ApplyImpulse { get; set; }
        public IGunInputDevice InputDevice { get; set; } = ZeroGunInputDevice.Instance;

        public void Equip(IGunInputDevice inputDevice)
        {
            InputDevice            = inputDevice;
            CircleInstance.Visible = true;
            MagazineBar.Visible    = true;
        }

        public void Unequip()
        {
            InputDevice            = ZeroGunInputDevice.Instance;
            CircleInstance.Visible = false;
            MagazineBar.Visible    = false;
        }
    }
}
