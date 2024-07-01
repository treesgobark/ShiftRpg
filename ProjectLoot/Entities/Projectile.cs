using ANLG.Utilities.FlatRedBall.Extensions;
using Microsoft.Xna.Framework;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.Effects;

namespace ProjectLoot.Entities
{
    public partial class Projectile
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

        public IEffectBundle OnHitTargetEffects { get; set; }
        public IEffectBundle OnHitHolderEffects { get; set; }
        public IEffectsComponent HolderEffectsComponent { get; set; }
        public bool IsActive { get; set; }
        public Team AppliesTo { get; set; }

        public void InitializeProjectile(float             projectileRadius, Vector3 projectileVelocity, Team appliesTo,
                                         IEffectBundle     targetHitEffects, IEffectBundle holderHitEffects,
                                         IEffectsComponent holderEffects)
        {
            CircleInstance.Radius  = projectileRadius;
            Velocity               = projectileVelocity;
            RotationZ              = projectileVelocity.XY().Angle() ?? 0;
            OnHitTargetEffects     = targetHitEffects;
            OnHitHolderEffects     = holderHitEffects;
            AppliesTo              = appliesTo;
            HolderEffectsComponent = holderEffects;
        }
    }
}
