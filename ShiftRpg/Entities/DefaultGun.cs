using ANLG.Utilities.FlatRedBall.Constants;
using ANLG.Utilities.FlatRedBall.Controllers;
using Microsoft.Xna.Framework;
using ShiftRpg.Contracts;
using ShiftRpg.Controllers.Gun;
using ShiftRpg.Effects;
using ShiftRpg.Factories;

namespace ShiftRpg.Entities
{
    public partial class DefaultGun
    {
        /// <summary>
        /// Initialization logic which is executed only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
        private void CustomInitialize()
        {
            Controllers = new ControllerCollection<IGun, GunController>();
            Controllers.Add(new Ready(this));
            Controllers.Add(new Recovery(this));
            Controllers.Add(new Reloading(this));
            Controllers.InitializeStartingController<Ready>();
        }

        private void CustomActivity()
        {
            Controllers.DoCurrentControllerActivity();
        }

        private void CustomDestroy()
        {
        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {
        }

        public override void Fire()
        {
            var dir = Vector2ExtensionMethods.FromAngle(Parent.RotationZ).NormalizedOrZero().ToVector3();
            if (dir == Vector3.Zero) return;
            
            var proj = BulletFactory.CreateNew();
            proj.Position = Position;
        
            // bullet.DamageToDeal          = data.Damage;
            proj.CircleInstance.Radius = CurrentGunData.ProjectileRadius;
            proj.Velocity              = dir * CurrentGunData.ProjectileSpeed;
            proj.ApplyHolderEffects    = ApplyHolderEffects;
            proj.TargetHitEffects      = TargetHitEffects;
            proj.HolderHitEffects      = HolderHitEffects;

            var effects = new[]
            {
                new KnockbackEffect(Team, Source, Guid.NewGuid(), 100, RotationZ + MathConstants.HalfTurn)
            };
            ApplyHolderEffects(effects);

            MagazineRemaining--;
        }
    }
}
