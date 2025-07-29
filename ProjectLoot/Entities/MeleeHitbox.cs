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

        public static MeleeHitboxBuilder CreateHitbox(IMeleeWeaponModel weaponModel)
        {
            MeleeHitbox? hitbox = MeleeHitboxFactory.CreateNew();
            
            weaponModel.MeleeWeaponComponent.AttachObjectToAttackOrigin(hitbox);
            hitbox.ParentRotationChangesPosition = false;
            hitbox.ParentRotationChangesRotation = false;
            hitbox.HolderEffectsComponent        = weaponModel.HolderEffects;
            hitbox.AppliesTo                     = ~weaponModel.MeleeWeaponComponent.Team;
            hitbox.TargetHitEffects              = new EffectBundle();
            hitbox.HolderHitEffects              = new EffectBundle();
            
            return new MeleeHitboxBuilder(hitbox);
        }
    }

    public class MeleeHitboxBuilder
    {
        private readonly MeleeHitbox _meleeHitbox;

        public MeleeHitboxBuilder(MeleeHitbox meleeHitbox)
        {
            _meleeHitbox = meleeHitbox;
        }
        
        public MeleeHitboxBuilder AddCircle(float radius, float relativeX = 0f)
        {
            var circle = new Circle
            {
                Radius                  = radius,
                RelativeX               = relativeX,
                Visible                 = false,
                IgnoresParentVisibility = true,
            };
            
            circle.AttachTo(_meleeHitbox);
            _meleeHitbox.Collision.Add(circle);
            return this;
        }

        public MeleeHitboxBuilder AddSpriteInfo(string chainName, TimeSpan duration = default)
        {
            _meleeHitbox.SpriteInstance.CurrentChainName = chainName;
            _meleeHitbox.SpriteInstance.AnimationSpeed   = duration != TimeSpan.Zero ? 0.99f / (float)duration.TotalSeconds : 1f;
            _meleeHitbox.SpriteInstance.RelativeZ        = 0.2f;
            return this;
        }

        public MeleeHitbox Build()
        {
            return _meleeHitbox;
        }
    }
}
