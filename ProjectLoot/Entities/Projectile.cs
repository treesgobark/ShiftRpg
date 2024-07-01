using ANLG.Utilities.FlatRedBall.Extensions;
using Microsoft.Xna.Framework;
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

        public IEffectBundle TargetHitEffects { get; set; }
        public IEffectBundle HolderHitEffects { get; set; }
        public IWeaponHolder Holder { get; set; }
        public bool IsActive { get; set; }
        public Team AppliesTo { get; set; }
        
        public void InitializeProjectile(float projectileRadius, Vector3 projectileVelocity, IEffectBundle targetHitEffects,
            IEffectBundle holderHitEffects, Team appliesTo)
        {
            CircleInstance.Radius = projectileRadius;
            Velocity              = projectileVelocity;
            RotationZ             = projectileVelocity.XY().Angle() ?? 0;
            TargetHitEffects      = targetHitEffects;
            HolderHitEffects      = holderHitEffects;
            AppliesTo             = appliesTo;
        }
        
        public void InitializeProjectile(float projectileRadius, Vector3 projectileVelocity, Team appliesTo)
        {
            CircleInstance.Radius = projectileRadius;
            Velocity              = projectileVelocity;
            RotationZ             = projectileVelocity.XY().Angle() ?? 0;
            AppliesTo             = appliesTo;
        }
    }
}
