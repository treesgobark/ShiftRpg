using FlatRedBall.Math.Geometry;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.Effects;
using ProjectLoot.Effects.Base;
using ProjectLoot.Factories;

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

        public bool IsActive { get; set; } = true;
        public IEffectBundle TargetHitEffects { get; set; } = EffectBundle.Empty;
        public IEffectBundle HolderHitEffects { get; set; } = EffectBundle.Empty;
        public IEffectsComponent HolderEffectsComponent { get; set; }
        public Team AppliesTo { get; set; }

        public static MeleeHitbox CreateHitbox(IMeleeWeaponModel weaponModel)
        {
            MeleeHitbox? hitbox = MeleeHitboxFactory.CreateNew();
            
            weaponModel.MeleeWeaponComponent.AttachObjectToAttackOrigin(hitbox);
            hitbox.ParentRotationChangesPosition   = false;
            hitbox.ParentRotationChangesRotation   = false;
            hitbox.HolderEffectsComponent = weaponModel.HolderEffects;
            hitbox.AppliesTo              = ~weaponModel.MeleeWeaponComponent.Team;
            
            return hitbox;
        }

        public MeleeHitbox AddSpriteInfo(string chainName, TimeSpan duration)
        {
            SpriteInstance.CurrentChainName = chainName;
            SpriteInstance.AnimationSpeed   = 0.99f / (float)duration.TotalSeconds;
            SpriteInstance.RelativeZ        = 0.2f;
            return this;
        }

        public MeleeHitbox AddCircle(float radius, float relativeX)
        {
            var circle = new Circle
            {
                Radius                  = radius,
                RelativeX               = relativeX,
                Visible                 = false,
                IgnoresParentVisibility = true,
            };
            
            circle.AttachTo(this);
            Collision.Add(circle);
            return this;
        }
    }
}
