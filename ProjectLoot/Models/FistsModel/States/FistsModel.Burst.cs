using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using ProjectLoot.Contracts;
using ProjectLoot.Controllers.ModularStates;
using ProjectLoot.Entities;

namespace ProjectLoot.Models;

public partial class FistsModel
{
    private class Burst : ModularState
    {
        public Burst(IReadonlyStateMachine outerStates, IMeleeWeaponModel model, ITimeManager timeManager)
        {
            AddModule(new StateMachineModule(outerStates))
               .AddSubstate(sm => new BurstExpand(sm, model, timeManager, this))
               .AddSubstate(sm => new BurstFade(sm, timeManager, this))
               .AddSubstate(_ => new BurstCleanup(this))
               .SetStartingState<BurstExpand>()
               .SetExitState<Idle>();
        }

        public MeleeHitbox? Hitbox { get; set; }
    }
}