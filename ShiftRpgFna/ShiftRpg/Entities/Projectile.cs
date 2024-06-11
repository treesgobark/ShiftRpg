using System;
using System.Collections.Generic;
using System.Text;
using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Graphics.Particle;
using FlatRedBall.Math.Geometry;
using Microsoft.Xna.Framework;
using ShiftRpg.Contracts;

namespace ShiftRpg.Entities
{
    public abstract partial class Projectile : IProjectile
    {
        /// <summary>
        /// Initialization logic which is executed only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
        private void CustomInitialize()
        {
            IsActive = true;
        }

        private void CustomActivity()
        {
        }

        private void CustomDestroy()
        {
            IsActive = false;
        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {
        }

        public IEffectBundle TargetHitEffects { get; set; }
        public IEffectBundle HolderHitEffects { get; set; }
        public bool IsActive { get; set; }
        
        public void InitializeProjectile(float projectileRadius, Vector3 projectileSpeed, IEffectBundle targetHitEffects,
            IEffectBundle holderHitEffects)
        {
            CircleInstance.Radius = projectileRadius;
            Velocity              = projectileSpeed;
            TargetHitEffects      = targetHitEffects;
            HolderHitEffects      = holderHitEffects;
        }
    }
}
