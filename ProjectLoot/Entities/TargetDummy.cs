using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using FlatRedBall;
using ProjectLoot.Components;
using ProjectLoot.Contracts;
using ProjectLoot.Effects;
using ProjectLoot.Handlers;

namespace ProjectLoot.Entities
{
    public partial class TargetDummy
    {
        public HealthComponent Health { get; private set; }
        public ShatterComponent Shatter { get; private set; }
        public WeaknessComponent Weakness { get; private set; }
        public TransformComponent Transform { get; private set; }
        public HitstopComponent Hitstop { get; private set; }
        
        /// <summary>
        /// Initialization logic which is executed only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
        private void CustomInitialize()
        {
            InitializeComponents();
            InitializeHandlers();
        }

        private void InitializeComponents()
        {
            Health    = new HealthComponent(MaxHealth, HealthBarRuntimeInstance);
            Shatter   = new ShatterComponent(HealthBarRuntimeInstance);
            Weakness  = new WeaknessComponent(HealthBarRuntimeInstance);
            Transform = new TransformComponent(this);
            Hitstop   = new HitstopComponent();
            
            Health.DamageModifiers.Upsert("weakness_damage_bonus", new StatModifier<float>(
                effect => Weakness.CurrentWeaknessPercentage > 0 && effect.Source.Contains(SourceTag.Gun),
                effect => 1 + Weakness.CurrentWeaknessPercentage * Weakness.DamageConversionRate,
                ModifierCategory.Multiplicative));
        }

        private void InitializeHandlers()
        {
            Effects.AddHandler(new AttackHandler(Effects, Health, FrbTimeManager.Instance));
            Effects.AddHandler(new HealthReductionHandler(Effects, Health, FrbTimeManager.Instance));
            Effects.AddHandler(new DamageNumberHandler(Effects, Transform));
            Effects.AddHandler(new ShatterDamageHandler(Effects, Health, Shatter));
            Effects.AddHandler(new ApplyShatterDamageHandler(Effects, Shatter, Health));
            Effects.AddHandler(new WeaknessDamageHandler(Effects, Weakness));
            Effects.AddHandler(new HitstopHandler(Effects, Hitstop, Transform, FrbTimeManager.Instance));
            // Effects.AddHandler(new KnockbackHandler(Effects, Transform));
            Effects.AddHandler(new KnockTowardHandler(Effects, Transform));
        }

        private void CustomActivity()
        {
            if (TimeSinceLastDamage > 2.0)
            {
                Health.CurrentHealth = MaxHealth;
                Shatter.CurrentShatterDamage = 0;
                Shatter.CurrentShatterPercentage = 0;
                Weakness.CurrentWeaknessPercentage = 0;
            }
        }

        private void CustomDestroy()
        {


        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {


        }

        private double TimeSinceLastDamage => TimeManager.CurrentScreenSecondsSince(Health.LastDamageTime.TotalSeconds);
    }
}
