using System.Diagnostics;
using ANLG.Utilities.Core.Extensions;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using ProjectLoot.DataTypes;

namespace ProjectLoot.Entities
{
    public partial class WeaponDrop
    {
        private TimeSpan LastTimePreviewed { get; set; }
        private TimeSpan PreviewLingerTime { get; } = TimeSpan.FromMilliseconds(200);
        
        public object ContainedWeapon { get; private set; }
        
        /// <summary>
        /// Initialization logic which is executed only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
        private void CustomInitialize()
        {
            // if (Random.Shared.NextBool())
            if (false)
            {
                ContainedWeapon = GlobalContent.GunData[GunData.OrderedList.ChooseRandom()];
            }
            else
            {
                ContainedWeapon = GlobalContent.MeleeWeaponData[MeleeWeaponData.OrderedList.ChooseRandom()];
            }
            
            PreviewSprite.CurrentChainName = ContainedWeapon switch
            {
                GunData gunData => gunData.GunName,
                MeleeWeaponData meleeWeaponData => meleeWeaponData.Name,
                _ => throw new UnreachableException("who the fuck put something else in here"),
            };
        }

        private void CustomActivity()
        {
            if (FrbTimeManager.Instance.TotalGameTime - LastTimePreviewed > PreviewLingerTime)
            {
                PreviewSprite.Visible = false;
            }
        }

        private void CustomDestroy() { }

        private static void CustomLoadStaticContent(string contentManagerName) { }

        public void ShowPreview()
        {
            PreviewSprite.Visible = true;
            LastTimePreviewed = FrbTimeManager.Instance.TotalGameTime;
        }
    }
}
