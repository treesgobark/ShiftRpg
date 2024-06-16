using ProjectLoot.Components.Interfaces;

namespace ProjectLoot.GumRuntimes
{
    public partial class HealthBarRuntime
    {
        partial void CustomInitialize () 
        {
        }

        public void Reset()
        {
            MainBarProgressPercentage = 100;
            ShatterBarProgressPercentage = 0;
            WeaknessBarProgressPercentage = 0;
        }
    }
}
