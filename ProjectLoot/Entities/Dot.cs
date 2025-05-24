using System;
using System.Collections.Generic;
using System.Text;
using ANLG.Utilities.Core.Extensions;
using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Debugging;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Graphics.Particle;
using FlatRedBall.Math.Geometry;
using Microsoft.Xna.Framework;
using ProjectLoot.Components;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.DataTypes;
using ProjectLoot.Effects;
using ProjectLoot.Factories;
using ProjectLoot.Handlers;
using ProjectLoot.Models;

namespace ProjectLoot.Entities
{
    public partial class Dot
    {
        private StateMachine States { get; set; }
        
        private TransformComponent Transform { get; set; }
        private HealthComponent Health { get; set; }
        private HitstopComponent Hitstop { get; set; }
        private SpriteComponent BodySpriteComponent { get; set; }
        private SpriteComponent SatelliteSpriteComponent { get; set; }
        private PoiseComponent Poise { get; set; }
        
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
            BodySpriteComponent      = new SpriteComponent(BodySprite);
            SatelliteSpriteComponent = new SpriteComponent(SatelliteSprite);
            Poise                    = new PoiseComponent { PoiseThreshold = PoiseThreshold };
        }

        private void InitializeHandlers()
        {
            Effects.AddHandler<HitstopEffect>(new HitstopHandler(Effects, Hitstop, Transform, FrbTimeManager.Instance, SatelliteSpriteComponent, Health));
            Effects.AddHandler<AttackEffect>(new AttackHandler(Effects, Health, FrbTimeManager.Instance));
            Effects.AddHandler<HealthReductionEffect>(new HealthReductionHandler(Effects, Health, FrbTimeManager.Instance, Hitstop, this));
            Effects.AddHandler<HealthReductionEffect>(new DamageNumberHandler(Effects, Transform));
            Effects.AddHandler<HealthReductionEffect>(new FlashOnDamageHandler(Effects, BodySpriteComponent, FrbTimeManager.Instance));
            Effects.AddHandler<HealthReductionEffect>(new FlashOnDamageHandler(Effects, SatelliteSpriteComponent, FrbTimeManager.Instance));
            Effects.AddHandler<KnockbackEffect>(new KnockbackHandler(Effects, Transform));
            Effects.AddHandler<PoiseDamageEffect>(new PoiseDamageHandler(Effects, Poise));
        }

        private void InitializeStates()
        {
            States = new StateMachine();
            States.Add(new Idle(States, FrbTimeManager.Instance, this));
            States.Add(new Windup(States, FrbTimeManager.Instance, this));
            States.Add(new Attacking(States, FrbTimeManager.Instance, this));
            States.InitializeStartingState<Idle>();
        }

        private void CustomActivity()
        {
            if (States.IsInitialized)
            {
                States.DoCurrentStateActivity();
            }
            
            if (Health.CurrentHealth <= 0)
            {
                Debugger.Log($"_Dot@{TimeManager.CurrentFrame}:Health:{Health.CurrentHealth}");
                // Destroy();
            }
        }

        private void CustomDestroy()
        {
            States.Uninitialize();
            
            var corpse = CorpseFactory.CreateNew(Position);
            corpse.InitializeFromEntity(this);
            // GlobalContent.ShotgunBlastQuick.Play(0.3f, Random.Shared.NextSingle(-0.2f, 0.2f), 0f);
        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {


        }
    }
}
