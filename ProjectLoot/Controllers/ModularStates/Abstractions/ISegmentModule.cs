namespace ProjectLoot.Controllers.ModularStates;

public interface ISegmentModule
{
    bool TryHandleSegment();
    int TotalSegments { get; }
    int CurrentSegmentIndex { get; }
}