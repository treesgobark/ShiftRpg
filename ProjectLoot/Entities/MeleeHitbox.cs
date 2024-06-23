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
using ProjectLoot.Contracts;
using ProjectLoot.Effects;

namespace ProjectLoot.Entities
{
    public partial class MeleeHitbox
    {
        /// <summary>
        /// Initialization logic which is executed only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
        private void CustomInitialize() { }

        private void CustomActivity() { }

        private void CustomDestroy() { }

        private static void CustomLoadStaticContent(string contentManagerName) { }
        
        public TimeSpan Lifetime { get; set; }
        public IEffectBundle TargetHitEffects { get; set; }
        public IEffectBundle HolderHitEffects { get; set; }
        public IWeaponHolder Holder { get; set; }
        public Team AppliesTo { get; set; }
    }
}