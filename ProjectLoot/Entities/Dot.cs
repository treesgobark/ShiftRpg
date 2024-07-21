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
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Graphics.Particle;
using FlatRedBall.Math.Geometry;
using Microsoft.Xna.Framework;
using ProjectLoot.Components;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.DataTypes;
using ProjectLoot.Effects;
using ProjectLoot.Effects.Handlers;
using ProjectLoot.Models;

namespace ProjectLoot.Entities
{
    public partial class Dot
    {
        private StateMachine States { get; set; }
        
        private TransformComponent TransformComponent { get; set; }
        private HealthComponent HealthComponent { get; set; }
        private HitstopComponent HitstopComponent { get; set; }
        private ISpriteComponent SpriteComponent { get; set; }
        
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
            TransformComponent = new TransformComponent(this, this);
            HealthComponent    = new HealthComponent(MaxHealth, HealthBarRuntimeInstance);
            HitstopComponent   = new HitstopComponent(() => CurrentMovement, m => CurrentMovement = m);
            SpriteComponent    = new SpriteComponent(SatelliteSprite);
        }

        private void InitializeHandlers()
        {
            Effects.HandlerCollection.Add<HitstopEffect>(new HitstopHandler(Effects, HitstopComponent, TransformComponent, FrbTimeManager.Instance, SpriteComponent), 0);
            Effects.HandlerCollection.Add<DamageEffect>(new DamageHandler(Effects, HealthComponent, TransformComponent, FrbTimeManager.Instance));
            Effects.HandlerCollection.Add<KnockbackEffect>(new KnockbackHandler(Effects, TransformComponent));
        }

        private void InitializeStates()
        {
            States = new StateMachine();
            States.Add(new Idle(States, FrbTimeManager.Instance, this));
            States.Add(new Attacking(States, FrbTimeManager.Instance, this));
            States.InitializeStartingState<Idle>();
        }

        private void CustomActivity()
        {
            States.DoCurrentStateActivity();
            
            if (HealthComponent.CurrentHealth <= 0)
            {
                Destroy();
            }
        }

        private void CustomDestroy()
        {


        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {


        }
    }
}
