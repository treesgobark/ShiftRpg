using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.Effects;
using ProjectLoot.Effects.Base;

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
    }
}
