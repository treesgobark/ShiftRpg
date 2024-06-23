using System.Collections.Generic;
using ANLG.Utilities.FlatRedBall.States;
using FlatRedBall.Screens;
using ProjectLoot.Components;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.Effects;
using ProjectLoot.Effects.Handlers;
using ProjectLoot.InputDevices;
using ProjectLoot.Screens;

namespace ProjectLoot.Entities
{
    public partial class DefaultRangedEnemy : IWeaponHolder
    {
        private WeaponsComponent Weapons { get; set; }
        public HealthComponent Health { get; private set; }
        public ShatterComponent Shatter { get; private set; }
        public WeaknessComponent Weakness { get; private set; }
        public HitstopComponent Hitstop { get; private set; }
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
            Health = new HealthComponent(MaxHealth, HealthBarRuntimeInstance);
            Shatter = new ShatterComponent(HealthBarRuntimeInstance);
            Weakness = new WeaknessComponent(HealthBarRuntimeInstance);
            Hitstop = new HitstopComponent(() => CurrentMovement, m => CurrentMovement = m);
            Weapons = new WeaponsComponent(EnemyInputDevice, Team.Enemy, this, this);
            
            Health.DamageModifiers.Upsert("weakness_damage_bonus", new StatModifier<float>(
                effect => Weakness.CurrentWeaknessPercentage > 0 && effect.Source.Contains(SourceTag.Gun),
                effect => 1 + Weakness.CurrentWeaknessPercentage * Weakness.DamageConversionRate,
                ModifierCategory.Multiplicative));
        }

        private void InitializeHandlers()
        {
            Effects.HandlerCollection.Add(new HitstopHandler(Effects, Hitstop, this, SpriteInstance), 0);
            Effects.HandlerCollection.Add(new DamageHandler(Effects, Health, this));
            Effects.HandlerCollection.Add(new ShatterDamageHandler(Effects, Health, Shatter));
            Effects.HandlerCollection.Add(new ApplyShatterDamageHandler(Effects, Shatter, Health));
            Effects.HandlerCollection.Add(new WeaknessDamageHandler(Effects, Health, Weakness));
            Effects.HandlerCollection.Add(new KnockbackHandler(Effects, this, Hitstop));
        }

        private void InitializeControllers()
        {
            StateMachine = new StateMachine();
            StateMachine.Add(new GunMode(this, StateMachine));
            StateMachine.InitializeStartingState<GunMode>();
        }

        private void CustomActivity()
        {
            if (Hitstop.IsStopped) { return; }
            
            Weapons.Activity();
            StateMachine.DoCurrentStateActivity();
            
            if (Health.CurrentHealth <= 0)
            {
                Destroy();
            }
        }

        private void CustomDestroy()
        {
            Weapons.Destroy();
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
