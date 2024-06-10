using System;
using System.Collections.Generic;
using ANLG.Utilities.FlatRedBall.Extensions;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using FlatRedBall.Debugging;
using FlatRedBall.Glue.StateInterpolation;
using FlatRedBall.Input;
using ShiftRpg.Contracts;
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
            MagazineBar.ProgressPercentage = 100f;
            MagazineBar.SubProgressPercentage = 0f;
        }

        private void CustomActivity()
        {
        }

        private void CustomDestroy() { }
        private static void CustomLoadStaticContent(string contentManagerName) { }
        public Team Team { get; set; }
        public SourceTag Source { get; set; } = SourceTag.Gun;
        
        // Implement IGun

        public IWeaponHolder Holder { get; set; }

        public IEffectBundle TargetHitEffects
        {
            get
            {
                var effects = new EffectBundle(~Team, Source);
                
                effects.AddEffect(new DamageEffect(~Team, Source, CurrentGunData.Damage));
                effects.AddEffect(new KnockbackEffect(~Team, Source, 100, this.GetRotationZ()));
                effects.AddEffect(new ShatterDamageEffect(~Team, Source, 1));

                return Holder.ModifyTargetEffects(effects);
            }
        }

        public IEffectBundle HolderHitEffects { get; set; } = EffectBundle.Empty;
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
