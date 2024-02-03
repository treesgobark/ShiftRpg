using System;
using Microsoft.Xna.Framework;
using ShiftRpg.Contracts;
using ShiftRpg.Factories;

namespace ShiftRpg.Entities
{
    public abstract partial class Gun : IGun
    {
        private int _magazineRemaining;

        public int MagazineRemaining
        {
            get => _magazineRemaining;
            set
            {
                _magazineRemaining = value;
                MagazineBar.ProgressPercentage = 100 * _magazineRemaining / (float)MagazineSize;
            }
        }

        /// <summary>
        /// Initialization logic which is executed only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
        private void CustomInitialize()
        {
            MagazineRemaining = MagazineSize;
            var hudParent = gumAttachmentWrappers[0];
            hudParent.ParentRotationChangesRotation = false;
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
        
        public void Equip()
        {
            CircleInstance.Visible = true;
            MagazineBar.Visible    = true;
        }

        public void Unequip()
        {
            CircleInstance.Visible = false;
            MagazineBar.Visible    = false;
        }
    }
}
