using ANLG.Utilities.FlatRedBall.Extensions;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using FlatRedBall.Glue.StateInterpolation;
using ProjectLoot.Components;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.DataTypes;
using ProjectLoot.Effects;
using ProjectLoot.InputDevices;
using StateInterpolationPlugin;

namespace ProjectLoot.Entities
{
    public abstract partial class Gun : IGun
    {
        public EffectsComponent Effects { get; private set; }
        public GunData CurrentGunData => GunDataCache.Obj;
        protected FrameCache<GunData> GunDataCache { get; } = new(() => GlobalContent.GunData[GunData.Pistol]);
        
        private int _magazineRemaining;
        private float _reloadProgress;

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

        public float ReloadProgress
        {
            get => _reloadProgress;
            set
            {
                _reloadProgress = value;
                MagazineBar.ProgressPercentage = _reloadProgress * 100f;
            }
        }

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

            Effects = new EffectsComponent();
        }

        private void CustomActivity()
        {
        }

        private void CustomDestroy() { }
        private static void CustomLoadStaticContent(string contentManagerName) { }
        public SourceTag Source { get; set; } = SourceTag.Gun;
        
        // Implement IGun

        public IWeaponHolder Holder { get; set; }

        public IEffectBundle TargetHitEffects
        {
            get
            {
                var effects = new EffectBundle(~Effects.Team, Source);
                
                effects.AddEffect(new DamageEffect(~Effects.Team, Source, CurrentGunData.Damage));
                effects.AddEffect(new KnockbackEffect(~Effects.Team, Source, 200, this.GetRotationZ(), KnockbackBehavior.Replacement));
                effects.AddEffect(new ShatterDamageEffect(~Effects.Team, Source, 3));
                effects.AddEffect(new HitstopEffect(~Effects.Team, Source, TimeSpan.FromMilliseconds(50)));

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
        }

        public void FillMagazine()
        {
            MagazineRemaining                  = MagazineSize;
            MagazineBar.ForegroundGreen = BarColor;
        }
    }
}
