using ProjectLoot.Components.Interfaces;
using ProjectLoot.DataTypes;

namespace ProjectLoot.Components;

public class HitstopComponent : IHitstopComponent
{
    private Func<TopDownValues>? GetMovementValues { get; }
    private Action<TopDownValues?>? SetMovementValues { get; }
    private TopDownValues? StoredMovementValues { get; set; }

    public HitstopComponent() { }
    
    public HitstopComponent(Func<TopDownValues> getMovementValues, Action<TopDownValues?> setMovementValues)
    {
        GetMovementValues = getMovementValues;
        SetMovementValues = setMovementValues;
    }
    
    public bool IsStopped { get; set; }
    public TimeSpan RemainingHitstopTime { get; set; }

    public void Stop()
    {
        if ((GetMovementValues, SetMovementValues) is (not null, not null))
        {
            StoredMovementValues = GetMovementValues();
            SetMovementValues(null);
        }
    }

    public void Resume()
    {
        if ((GetMovementValues, SetMovementValues) is (not null, not null))
        {
            SetMovementValues(StoredMovementValues);
        }
    }
}