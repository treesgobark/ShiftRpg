using System.Diagnostics;
using ANLG.Utilities.Core;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using ANLG.Utilities.States;
using FlatRedBall;
using ProjectLoot.Components;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.DataTypes;
using ProjectLoot.Effects;
using ProjectLoot.Effects.Base;
using ProjectLoot.Handlers;
using ProjectLoot.InputDevices;
using ProjectLoot.Models;

namespace ProjectLoot.Entities
{
    public partial class DefaultRangedEnemy : IWeaponHolder
    {
        private TransformComponent TransformComponent { get; set; }
        private HealthComponent HealthComponent { get; set; }
        private ShatterComponent ShatterComponent { get; set; }
        private WeaknessComponent WeaknessComponent { get; set; }
        private HitstopComponent HitstopComponent { get; set; }
        private GunComponent GunComponent { get; set; }
        private ISpriteComponent SpriteComponent { get; set; }
        
        private StateMachine States { get; set; }
        
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
            TransformComponent = new TransformComponent(this, this);
            HealthComponent    = new HealthComponent(MaxHealth, HealthBarRuntimeInstance);
            ShatterComponent   = new ShatterComponent(HealthBarRuntimeInstance);
            WeaknessComponent  = new WeaknessComponent(HealthBarRuntimeInstance);
            HitstopComponent   = new HitstopComponent(() => CurrentMovement, m => CurrentMovement = m);
            GunComponent       = new GunComponent(Team.Enemy, EnemyInputDevice, GunSprite, this);
            SpriteComponent    = new SpriteComponent(SpriteInstance);
            
            GunComponent.Add(new StandardGunModel(GlobalContent.GunData[GunData.OrderedList.ChooseRandom()], GunComponent, GunComponent, Effects));
            
            HealthComponent.DamageModifiers.Upsert("weakness_damage_bonus", new StatModifier<float>(
                effect => WeaknessComponent.CurrentWeaknessPercentage > 0 && effect.Source.Contains(SourceTag.Gun),
                effect => 1 + WeaknessComponent.CurrentWeaknessPercentage * WeaknessComponent.DamageConversionRate,
                ModifierCategory.Multiplicative));
        }

        private void InitializeHandlers()
        {
            Effects.AddHandler(new HitstopHandler(Effects, HitstopComponent, TransformComponent, FrbTimeManager.Instance, SpriteComponent));
            Effects.AddHandler(new AttackHandler(Effects, HealthComponent, FrbTimeManager.Instance));
            Effects.AddHandler(new ShatterDamageHandler(Effects, HealthComponent, ShatterComponent));
            Effects.AddHandler(new ApplyShatterDamageHandler(Effects, ShatterComponent, HealthComponent));
            Effects.AddHandler(new WeaknessDamageHandler(Effects, WeaknessComponent));
            Effects.AddHandler(new KnockbackHandler(Effects, TransformComponent));
        }

        private void InitializeControllers()
        {
            States = new StateMachine();
            States.Add(new GunMode(this, States, FrbTimeManager.Instance));
            States.InitializeStartingState<GunMode>();
        }

        private void CustomActivity()
        {
            if (HitstopComponent.IsStopped) { return; }
            
            GunComponent.Activity();
            States.DoCurrentStateActivity();
            
            if (HealthComponent.CurrentHealth <= 0)
            {
                Destroy();
            }

            if (XVelocity != 0)
            {
                Debug.WriteLine($"_Enemy@{TimeManager.CurrentFrame}:XVelocity:{XVelocity}:XAcceleration:{XAcceleration}:InputX:{MovementInput.X}:InputY:{MovementInput.Y}");
            }
        }

        private void CustomDestroy()
        {
            States.Uninitialize();
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
