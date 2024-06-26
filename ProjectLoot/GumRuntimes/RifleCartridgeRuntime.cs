using ProjectLoot.Models;

namespace ProjectLoot.GumRuntimes
{
    public partial class RifleCartridgeRuntime : ICartridgeDisplay
    {
        public float XPosition { get => X; set => X = value; }
        public float YPosition { get => Y; set => Y = value; }
        public float ZPosition { get => Z; set => Z = value; }
        
        public float XVelocity { get; set; }
        public float YVelocity { get; set; }
        
        public float YAcceleration { get; set; }
        
        public float ZRotation { get => Rotation; set => Rotation = value; }
        public float ZRotationVelocity { get; set; }
        
        public float DestructionCountdown { get; set; }
        
        partial void CustomInitialize () 
        {
        }

        public void SetEmpty()
        {
            CurrentIsSpentState = IsSpent.Empty;
        }

        public void SetFilled()
        {
            CurrentIsSpentState = IsSpent.NotSpent;
        }

        public void SetSpent()
        {
            CurrentIsSpentState = IsSpent.Spent;
        }
    }
}
