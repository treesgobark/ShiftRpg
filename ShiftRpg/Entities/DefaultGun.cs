using Microsoft.Xna.Framework;
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


        }

        private void CustomActivity()
        {


        }

        private void CustomDestroy()
        {


        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {


        }

        public override void BeginFire()
        {
            var dir = Vector2ExtensionMethods.FromAngle(RotationZ).NormalizedOrZero().ToVector3();
            if (dir != Vector3.Zero)
            {
                var bullet = BulletFactory.CreateNew();
                bullet.Position = Position;
                bullet.Velocity = dir * 500;
            }
        }

        public override void EndFire()
        {
        }
    }
}
