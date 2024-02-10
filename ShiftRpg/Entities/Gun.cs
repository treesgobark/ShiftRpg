using System;
using System.Collections.Generic;
using ANLG.Utilities.FlatRedBall.Controllers;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using FlatRedBall.Debugging;
using FlatRedBall.Input;
using ShiftRpg.Contracts;
using ShiftRpg.Controllers.Gun;
using ShiftRpg.DataTypes;
using ShiftRpg.Effects;
using ShiftRpg.InputDevices;

namespace ShiftRpg.Entities
{
    public abstract partial class Gun : IGun, IHasControllers<Gun, GunController>
    {
        public GunData CurrentGunData { get; set; } = GlobalContent.GunData[GunData.Pistol];
        
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

        public int MagazineSize { get; set; }
        public double ReloadTimeSeconds { get; set; }

        /// <summary>
        /// Initialization logic which is executed only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
        private void CustomInitialize()
        {
            MagazineSize      = CurrentGunData.MagazineSize;
            MagazineRemaining = MagazineSize;
            ReloadTimeSeconds = CurrentGunData.ReloadTime;
            
            var hudParent = gumAttachmentWrappers[0];
            hudParent.ParentRotationChangesRotation = false;
        }

        private void CustomActivity()
        {
        }

        private void CustomDestroy() { }
        private static void CustomLoadStaticContent(string contentManagerName) { }
        public Team Team { get; set; }
        public SourceTag Source { get; set; } = SourceTag.Gun;

        // Implement IHasControllers
        
        public ControllerCollection<Gun, GunController> Controllers { get; protected set; }
        
        // Implement IGun

        public Action<IReadOnlyList<IEffect>> ApplyHolderEffects { get; set; }
        public Action<IReadOnlyList<IEffect>> ModifyTargetEffects { get; set; }

        public IReadOnlyList<IEffect> TargetHitEffects
        {
            get
            {
                var effects = new IEffect[]
                {
                    new DamageEffect(~Team, Source, Guid.NewGuid(), 2),
                    new KnockbackEffect(~Team, Source, Guid.NewGuid(), 100, RotationZ),
                };
                ModifyTargetEffects(effects);
                return effects;
            }
        }

        public IReadOnlyList<IEffect> HolderHitEffects { get; set; } = new List<IEffect>();
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

        public abstract Projectile SpawnProjectile();
    }
}
