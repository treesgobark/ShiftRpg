using ProjectLoot.Components.Interfaces;

namespace ProjectLoot.Components;

public class HitstunComponent : IHitstunComponent
{
    public bool IsStunned { get; set; }
    public TimeSpan RemainingHitstunDuration { get; set; }
    
    public void Stun()
    {
        throw new NotImplementedException();
    }

    public void Recover()
    {
        throw new NotImplementedException();
    }
}