using System;
using System.Collections.Generic;
using ANLG.Utilities.FlatRedBall.Controllers;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using FlatRedBall.Debugging;
using FlatRedBall.Glue.StateInterpolation;
using FlatRedBall.Input;
using ShiftRpg.Contracts;
using ShiftRpg.Controllers.Gun;
using ShiftRpg.DataTypes;
using ShiftRpg.Effects;
using ShiftRpg.InputDevices;
using StateInterpolationPlugin;

namespace ShiftRpg.Entities
{
    public abstract partial class Gun : IGun
    {
        public GunData CurrentGunData => GunDataCache.Obj;
        protected FrameCache<GunData> GunDataCache { get; } = new(() => GlobalContent.GunData[GunData.Pistol]);
        
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

        public int MagazineSize => CurrentGunData.MagazineSize;
        public TimeSpan TimePerRound => TimeSpan.FromSeconds(CurrentGunData.SecondsPerRound);
        public TimeSpan ReloadTime => TimeSpan.FromSeconds(CurrentGunData.ReloadTime);
        public FiringType FiringType => CurrentGunData.IsSingleShot ? FiringType.Semiautomatic : FiringType.Automatic;
        private int BarColor { get; set; }

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
            ParentRotationChangesRotation           = true;
        }

        private void CustomActivity()
        {
        }

        private void CustomDestroy() { }
        private static void CustomLoadStaticContent(string contentManagerName) { }
        public Team Team { get; set; }
        public SourceTag Source { get; set; } = SourceTag.Gun;

        // Implement IHasControllers
        
        public ControllerCollection<IGun, GunController> Controllers { get; protected set; }
        
        // Implement IGun

        public Action<IReadOnlyList<IEffect>> ApplyHolderEffects { get; set; }
        public Action<IReadOnlyList<IEffect>> ModifyTargetEffects { get; set; }

        public IReadOnlyList<IEffect> TargetHitEffects
        {
            get
            {
                var effects = new IEffect[]
                {
                    new DamageEffect(~Team, Source, Guid.NewGuid(), CurrentGunData.Damage),
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

        public abstract void Fire();
        
        public void StartReload()
        {
            BarColor                           = MagazineBar.ForegroundGreen;
            MagazineBar.ForegroundGreen = 150;
            
            this.TweenAsync(p => MagazineBar.ProgressPercentage = p, 0, 100, (float)ReloadTime.TotalSeconds, InterpolationType.Linear, Easing.InOut);
        }

        public void FillMagazine()
        {
            MagazineRemaining                  = MagazineSize;
            MagazineBar.ForegroundGreen = BarColor;
        }
    }
}
