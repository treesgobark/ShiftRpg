using ANLG.Utilities.States;

namespace ProjectLoot.Controllers.ModularStates;

public class SegmentModule : IActivate, ISegmentModule
{
    private readonly IDurationModule _durationModule;
    private int _segmentsHandled;
    
    private int GoalSegmentsHandled => Math.Clamp((int)(_durationModule.NormalizedProgress * TotalSegments) + 1, 0, TotalSegments);

    public SegmentModule(IDurationModule durationModule, int segments)
    {
        _durationModule = durationModule;
        TotalSegments   = segments;
    }

    public int TotalSegments { get; }
    
    public int CurrentSegmentIndex => _segmentsHandled - 1;

    public bool TryHandleSegment()
    {
        if (GoalSegmentsHandled > _segmentsHandled)
        {
            _segmentsHandled++;
            return true;
        }
        
        return false;
    }
    
    public void OnActivate()
    {
        _segmentsHandled = 0;
    }
}