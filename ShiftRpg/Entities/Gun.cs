using System;
using Microsoft.Xna.Framework;
using ShiftRpg.Contracts;
using ShiftRpg.Factories;

namespace ShiftRpg.Entities
{
    public abstract partial class Gun : IGun
    {
        public int MagazineRemaining { get; set; }
        
        /// <summary>
        /// Initialization logic which is executed only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
        private void CustomInitialize()
        {
            MagazineRemaining = MagazineSize;
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

        public abstract void BeginFire();
        public abstract void EndFire();
        public abstract void Reload();
    }
}
