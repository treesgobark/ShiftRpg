namespace ProjectLoot.Components.Interfaces;

public interface IHitstunComponent
{
    bool IsStunned { get; set; }
    TimeSpan RemainingHitstunDuration { get; set; }
}