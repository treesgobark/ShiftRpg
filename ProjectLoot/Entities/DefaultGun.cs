using ANLG.Utilities.FlatRedBall.Constants;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using ANLG.Utilities.FlatRedBall.States;
using Microsoft.Xna.Framework;
using ProjectLoot.Contracts;
using ProjectLoot.Effects;
using ProjectLoot.Factories;

namespace ProjectLoot.Entities
{
    public partial class DefaultGun
    {
        protected StateMachine StateMachine { get; set; }
        
        /// <summary>
        /// Initialization logic which is executed only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
        private void CustomInitialize()
        {
            StateMachine = new StateMachine();
            StateMachine.Add(new Ready(this, StateMachine));
            StateMachine.Add(new Recovery(this, StateMachine));
            StateMachine.Add(new Reloading(this, StateMachine));
            StateMachine.InitializeStartingState<Ready>();
        }

        private void CustomActivity()
        {
            StateMachine.DoCurrentStateActivity();
        }

        private void CustomDestroy() { }

        private static void CustomLoadStaticContent(string contentManagerName) { }

        public override void Fire()
        {
            var dir = Vector2ExtensionMethods.FromAngle(Parent.RotationZ).NormalizedOrZero().ToVector3();
            if (dir == Vector3.Zero) return;
            
            var proj = BulletFactory.CreateNew(Position);
            proj.InitializeProjectile(CurrentGunData.ProjectileRadius, dir * CurrentGunData.ProjectileSpeed, TargetHitEffects, HolderHitEffects, Holder, ~Effects.Team);

            var effects = new EffectBundle(Effects.Team, Source);
            effects.AddEffect(new KnockbackEffect(Effects.Team, Source, 50, Rotation.FromRadians(RotationZ + MathConstants.HalfTurn), KnockbackBehavior.Additive));
            Holder.Effects.Handle(effects);

            MagazineRemaining--;
        }
    }
}
