using ANLG.Utilities.FlatRedBall.Constants;
using ANLG.Utilities.FlatRedBall.Controllers;
using ANLG.Utilities.FlatRedBall.States;
using Microsoft.Xna.Framework;
using ShiftRpg.Contracts;
using ShiftRpg.Effects;
using ShiftRpg.Factories;

namespace ShiftRpg.Entities
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
            proj.InitializeProjectile(CurrentGunData.ProjectileRadius, dir * CurrentGunData.ProjectileSpeed, ApplyHolderEffects, TargetHitEffects, HolderHitEffects);

            var effects = new[]
            {
                new KnockbackEffect(Team, Source, 100, RotationZ + MathConstants.HalfTurn)
            };
            ApplyHolderEffects(effects);

            MagazineRemaining--;
        }
    }
}
