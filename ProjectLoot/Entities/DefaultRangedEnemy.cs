using ANLG.Utilities.Core.States;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using ProjectLoot.Components;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.DataTypes;
using ProjectLoot.Effects;
using ProjectLoot.Effects.Handlers;
using ProjectLoot.InputDevices;
using ProjectLoot.Models;

namespace ProjectLoot.Entities
{
    public partial class DefaultRangedEnemy : IWeaponHolder
    {
        public TransformComponent TransformComponent { get; private set; }
        public HealthComponent HealthComponent { get; private set; }
        public ShatterComponent ShatterComponent { get; private set; }
        public WeaknessComponent WeaknessComponent { get; private set; }
        public HitstopComponent HitstopComponent { get; private set; }
        public GunComponent GunComponent { get; private set; }
        public ISpriteComponent SpriteComponent { get; private set; }
        
        private StateMachine StateMachine { get; set; }
        
        /// <summary>
        /// Initialization logic which is executed only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
        private void CustomInitialize()
        {
            InitializeInputs();
            InitializeComponents();
            InitializeHandlers();
            InitializeControllers();
        }

        private void InitializeInputs()
        {
            EnemyInputDevice = new RangedEnemyInputDevice(this, 100, 25);
            InitializeTopDownInput(EnemyInputDevice);
        }

        private void InitializeComponents()
        {
            TransformComponent = new TransformComponent(this);
            HealthComponent    = new HealthComponent(MaxHealth, HealthBarRuntimeInstance);
            ShatterComponent   = new ShatterComponent(HealthBarRuntimeInstance);
            WeaknessComponent  = new WeaknessComponent(HealthBarRuntimeInstance);
            HitstopComponent   = new HitstopComponent(() => CurrentMovement, m => CurrentMovement = m);
            GunComponent       = new GunComponent(GunSprite, Team.Enemy, EnemyInputDevice);
            SpriteComponent    = new SpriteComponent(SpriteInstance);
            
            GunComponent.Add(new StandardGunModel(GlobalContent.GunData["Rifle"], GunComponent, GunComponent, Effects));
            
            HealthComponent.DamageModifiers.Upsert("weakness_damage_bonus", new StatModifier<float>(
                effect => WeaknessComponent.CurrentWeaknessPercentage > 0 && effect.Source.Contains(SourceTag.Gun),
                effect => 1 + WeaknessComponent.CurrentWeaknessPercentage * WeaknessComponent.DamageConversionRate,
                ModifierCategory.Multiplicative));
        }

        private void InitializeHandlers()
        {
            Effects.HandlerCollection.Add<HitstopEffect>(new HitstopHandler(Effects, HitstopComponent, TransformComponent, FrbTimeManager.Instance, SpriteComponent), 0);
            Effects.HandlerCollection.Add<DamageEffect>(new DamageHandler(Effects, HealthComponent, TransformComponent, FrbTimeManager.Instance));
            Effects.HandlerCollection.Add<ShatterDamageEffect>(new ShatterDamageHandler(Effects, HealthComponent, ShatterComponent));
            Effects.HandlerCollection.Add<ApplyShatterDamageHandler>(new ApplyShatterDamageHandler(Effects, ShatterComponent, HealthComponent));
            Effects.HandlerCollection.Add<WeaknessDamageEffect>(new WeaknessDamageHandler(Effects, HealthComponent, WeaknessComponent));
            Effects.HandlerCollection.Add<KnockbackEffect>(new KnockbackHandler(Effects, TransformComponent, HitstopComponent));
        }

        private void InitializeControllers()
        {
            StateMachine = new StateMachine();
            StateMachine.Add(new GunMode(this, StateMachine, FrbTimeManager.Instance));
            StateMachine.InitializeStartingState<GunMode>();
        }

        private void CustomActivity()
        {
            if (HitstopComponent.IsStopped) { return; }
            
            GunComponent.Activity();
            StateMachine.DoCurrentStateActivity();
            
            if (HealthComponent.CurrentHealth <= 0)
            {
                Destroy();
            }
        }

        private void CustomDestroy()
        {
            
        }

        private static void CustomLoadStaticContent(string contentManagerName) { }

        #region IWeaponHolder
        
        IEffectsComponent IWeaponHolder.Effects => Effects;
        
        public void SetInputEnabled(bool isEnabled)
        {
            InputEnabled = isEnabled;
            EnemyInputDevice.InputEnabled = isEnabled;
        }

        public IEffectBundle ModifyTargetEffects(IEffectBundle effects) => effects;

        #endregion
    }
}
