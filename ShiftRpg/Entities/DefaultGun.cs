using ANLG.Utilities.FlatRedBall.Controllers;
using Microsoft.Xna.Framework;
using ShiftRpg.Contracts;
using ShiftRpg.Controllers.Gun;
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
            Controllers = new ControllerCollection<Gun, GunController>();
            Controllers.Add(new Ready(this));
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

        public override Projectile SpawnProjectile()
        {
            return BulletFactory.CreateNew();
        }
    }
}
