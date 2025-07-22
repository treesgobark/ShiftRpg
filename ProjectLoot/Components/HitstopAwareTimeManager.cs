using ANLG.Utilities.Core;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using ProjectLoot.Components.Interfaces;

namespace ProjectLoot.Components;

public class HitstopAwareTimeManager : ITimeManager
{
    private readonly IHitstopComponent hitstopComponent;

    public HitstopAwareTimeManager(IHitstopComponent hitstopComponent)
    {
        this.hitstopComponent = hitstopComponent;
    }

    public TimeSpan GameTimeSinceLastFrame => hitstopComponent.IsStopped
        ? TimeSpan.Zero
        : FrbTimeManager.Instance.GameTimeSinceLastFrame;
    public TimeSpan TotalGameTime => FrbTimeManager.Instance.TotalGameTime;
}