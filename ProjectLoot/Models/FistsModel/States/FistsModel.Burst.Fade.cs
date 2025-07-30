using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using ProjectLoot.Controllers.ModularStates;

namespace ProjectLoot.Models;

public partial class FistsModel
{
    private class BurstFade : ModularDelegateState
    {
        public BurstFade(IReadonlyStateMachine states, ITimeManager timeManager, Burst burst)
        {
            DurationModule durationModule = AddModule(new DurationModule(timeManager, TimeSpan.FromMilliseconds(120)));
            AddUpdate(() =>
            {
                if (burst.Hitbox is not null)
                {
                    burst.Hitbox.SpriteInstance.Alpha = 1 - durationModule.NormalizedProgress;
                }

                if (durationModule.NormalizedProgress >= 0 && burst.Hitbox != null)
                {
                    burst.Hitbox.IsActive = false;
                }
            });
            AddModule(new DurationExitModule<BurstCleanup>(durationModule, states));
        }
    }
}