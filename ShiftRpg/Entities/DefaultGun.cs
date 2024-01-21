using ANLG.Utilities.FlatRedBall.Controllers;
using Microsoft.Xna.Framework;
using ShiftRpg.Controllers.DefaultGun;
using ShiftRpg.Factories;

namespace ShiftRpg.Entities
{
    public partial class DefaultGun : IHasControllers<DefaultGun, DefaultGunController>
    {
        public ControllerCollection<DefaultGun, DefaultGunController> Controllers => DefaultGunControllers;
        private DefaultGunControllerCollection DefaultGunControllers { get; set; }
        
        /// <summary>
        /// Initialization logic which is executed only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
        private void CustomInitialize()
        {
            DefaultGunControllers = new DefaultGunControllerCollection();
            DefaultGunControllers.Add(new DefaultGunController(this));
            DefaultGunControllers.Add(new Reloading(this));
            DefaultGunControllers.InitializeStartingController<DefaultGunController>();
        }

        private void CustomActivity()
        {
            DefaultGunControllers.DoCurrentControllerActivity();
        }

        private void CustomDestroy()
        {
        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {


        }

        public override void BeginFire() => DefaultGunControllers.CurrentController.BeginFire();
        public override void EndFire() => DefaultGunControllers.CurrentController.EndFire();
        public override void Reload() => DefaultGunControllers.CurrentController.Reload();
    }
}
