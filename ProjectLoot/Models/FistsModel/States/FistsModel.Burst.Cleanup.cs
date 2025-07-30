using ANLG.Utilities.States;
using ProjectLoot.Controllers.ModularStates;

namespace ProjectLoot.Models;

public partial class FistsModel
{
    private class BurstCleanup : ModularDelegateState
    {
        public BurstCleanup(Burst burst)
        {
            AddActivate(() => burst.Hitbox?.Destroy());
            AddExitCondition(() => EmptyState.Instance);
        }
    }
}