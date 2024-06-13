namespace ShiftRpg.GumRuntimes
{
    public partial class HealthBarRuntime
    {
        partial void CustomInitialize () 
        {
        }

        public void SetAllToZero()
        {
            MainBarProgressPercentage     = 0;
            ShatterBarProgressPercentage  = 0;
            WeaknessBarProgressPercentage = 0;
        }

        public void SetAllToFull()
        {
            MainBarProgressPercentage     = 100;
            ShatterBarProgressPercentage  = 100;
            WeaknessBarProgressPercentage = 100;
        }

        public void Reset()
        {
            MainBarProgressPercentage     = 100;
            ShatterBarProgressPercentage  = 0;
            WeaknessBarProgressPercentage = 0;
        }
    }
}
