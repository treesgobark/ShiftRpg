using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using ProjectLoot.Contracts;
using ProjectLoot.Controllers.ModularStates;

namespace ProjectLoot.Models.SwordModel;

public class Spin : ModularState
{
    public Spin(ITimeManager timeManager, IReadonlyStateMachine outerStateMachine, IMeleeWeaponModel weaponModel)
    {
        AddModule(new StateMachineModule(outerStateMachine))
           .AddSubstate(sm => new SpinWindup(timeManager, sm, weaponModel))
           .AddSubstate(sm => new SpinActive(timeManager, sm, weaponModel))
           .SetStartingState<SpinWindup>()
           .SetExitState<Idle>();
    }
}
