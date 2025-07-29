using ANLG.Utilities.Core;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using ANLG.Utilities.States;
using FlatRedBall;
using FlatRedBall.Debugging;
using ProjectLoot.Components;
using ProjectLoot.Handlers;

namespace ProjectLoot.Entities
{
    public partial class Dot
    {
        private StateMachine States { get; set; }
        
        private TransformComponent Transform { get; set; }
        private HealthComponent Health { get; set; }
        private HitstopComponent Hitstop { get; set; }
        private DamageableSpriteComponent BodySpriteComponent { get; set; }
        private SpriteComponent SatelliteSpriteComponent { get; set; }
        private PoiseComponent Poise { get; set; }
        private CorpseInformationComponent CorpseInformationComponent { get; set; }
        
        public PositionedObject Target { get; set; }
        
        /// <summary>
        /// Initialization logic which is executed only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
        private void CustomInitialize()
        {
            InitializeComponents();
            InitializeHandlers();
            InitializeStates();
        }

        private void InitializeComponents()
        {
            Transform                = new TransformComponent(this, this);
            Health                   = new HealthComponent(MaxHealth, HealthBarRuntimeInstance);
            Hitstop                  = new HitstopComponent(() => CurrentMovement, m => CurrentMovement = m);
            BodySpriteComponent      = new DamageableSpriteComponent(BodySprite);
            SatelliteSpriteComponent = new SpriteComponent(SatelliteSprite);
            Poise                    = new PoiseComponent { PoiseThreshold = PoiseThreshold };
            
            CorpseInformationComponent = new CorpseInformationComponent
            {
                BodyAnimationChains      = DotAnimations,
                BodyChainName            = IsBig ? "BigBlueIdle" : "BlueIdle",
                ExplosionAnimationChains = DotAnimations,
                ExplosionChainName       = "BlueExplode",
            };
        }

        private void InitializeHandlers()
        {
            Effects.AddHandler(new HitstopHandler(Effects, Hitstop, Transform, FrbTimeManager.Instance, SatelliteSpriteComponent, Health));
            Effects.AddHandler(new AttackHandler(Effects, Health, FrbTimeManager.Instance));
            Effects.AddHandler(new HealthReductionHandler(Effects, Health, FrbTimeManager.Instance));
            Effects.AddHandler(new DamageNumberHandler(Effects, Transform));
            Effects.AddHandler(new FlashOnDamageHandler(Effects, BodySpriteComponent, FrbTimeManager.Instance));
            Effects.AddHandler(new FlashOnDamageHandler(Effects, SatelliteSpriteComponent, FrbTimeManager.Instance));
            Effects.AddHandler(new KnockbackHandler(Effects, Transform));
            Effects.AddHandler(new KnockTowardHandler(Effects, Transform));
            Effects.AddHandler(new PoiseDamageHandler(Effects, Poise));
            Effects.AddHandler(new CorpseSpawnHandler(Effects, Transform, CorpseInformationComponent, Hitstop));
            Effects.AddHandler(new DestructionHandler(Effects, this));
        }

        private void InitializeStates()
        {
            States = new StateMachine();
            States.Add(new Idle(States, FrbTimeManager.Instance, this));
            States.Add(new Windup(States, FrbTimeManager.Instance, this));
            States.Add(new Attacking(States, FrbTimeManager.Instance, this));
            States.SetStartingState<Idle>();
        }

        private void CustomActivity()
        {
            States.DoCurrentStateActivity();
            
            if (Health.CurrentHealth <= 0)
            {
                Debugger.Log($"_Dot@{TimeManager.CurrentFrame}:Health:{Health.CurrentHealth}");
            }
        }

        private void CustomDestroy()
        {
            States.ShutDown();
        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {


        }
    }
}
