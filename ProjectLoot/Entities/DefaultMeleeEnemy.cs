using ANLG.Utilities.Core.States;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using ProjectLoot.Components;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.Effects;
using ProjectLoot.Effects.Base;
using ProjectLoot.Handlers;
using ProjectLoot.InputDevices;

namespace ProjectLoot.Entities
{
    public partial class DefaultMeleeEnemy : IWeaponHolder
    {
        public TransformComponent TransformComponent { get; private set; }
        public HealthComponent HealthComponent { get; private set; }
        public ShatterComponent ShatterComponent { get; private set; }
        public WeaknessComponent WeaknessComponent { get; private set; }
        public HitstopComponent HitstopComponent { get; private set; }
        public MeleeWeaponComponent MeleeWeaponComponent { get; private set; }
        public SpriteComponent SpriteComponent { get; private set; }

        public StateMachine States { get; protected set; }
        
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
            EnemyInputDevice = new EnemyInputDevice(this);
            InitializeTopDownInput(EnemyInputDevice);
        }

        private void InitializeComponents()
        {
            TransformComponent   = new TransformComponent(this, this);
            HealthComponent      = new HealthComponent(MaxHealth, HealthBarRuntimeInstance);
            ShatterComponent     = new ShatterComponent(HealthBarRuntimeInstance);
            WeaknessComponent    = new WeaknessComponent(HealthBarRuntimeInstance);
            HitstopComponent     = new HitstopComponent(() => CurrentMovement, m => CurrentMovement = m);
            MeleeWeaponComponent = new MeleeWeaponComponent(Team.Enemy, EnemyInputDevice, this, this, MeleeWeaponSprite, SpriteInstance);
            SpriteComponent      = new SpriteComponent(SpriteInstance);
            
            HealthComponent.DamageModifiers.Upsert("weakness_damage_bonus", new StatModifier<float>(
                effect => WeaknessComponent.CurrentWeaknessPercentage > 0 && effect.Source.Contains(SourceTag.Gun),
                effect => 1 + WeaknessComponent.CurrentWeaknessPercentage * WeaknessComponent.DamageConversionRate,
                ModifierCategory.Multiplicative));
        }

        private void InitializeHandlers()
        {
            Effects.AddHandler<HitstopEffect>(new HitstopHandler(Effects, HitstopComponent, TransformComponent, FrbTimeManager.Instance, SpriteComponent));
            Effects.AddHandler<AttackEffect>(new AttackHandler(Effects, HealthComponent, FrbTimeManager.Instance));
            Effects.AddHandler<ShatterDamageEffect>(new ShatterDamageHandler(Effects, HealthComponent, ShatterComponent));
            Effects.AddHandler<ApplyShatterEffect>(new ApplyShatterDamageHandler(Effects, ShatterComponent, HealthComponent));
            Effects.AddHandler<WeaknessDamageHandler>(new WeaknessDamageHandler(Effects, WeaknessComponent));
            Effects.AddHandler<KnockbackHandler>(new KnockbackHandler(Effects, TransformComponent));
        }

        private void InitializeControllers()
        {
            States = new StateMachine();
            States.Add(new MeleeMode(this, States, FrbTimeManager.Instance));
            States.InitializeStartingState<MeleeMode>();
        }

        private void CustomActivity()
        {
            if (HitstopComponent.IsStopped) { return; }
            
            States.DoCurrentStateActivity();
            
            if (HealthComponent.CurrentHealth <= 0)
            {
                Destroy();
            }
        }

        private void CustomDestroy()
        {
            States.Uninitialize();
        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {
        }

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
