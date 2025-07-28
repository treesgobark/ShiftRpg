using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using ProjectLoot.Contracts;
using ProjectLoot.Controllers.ModularStates;

namespace ProjectLoot.Models.SwordModel;

public class Spin : ModularState
{
    public Spin(ITimeManager timeManager, IReadonlyStateMachine outerStateMachine, IMeleeWeaponModel weaponModel)
    {
        var smm = AddModule(new StateMachineModule<SpinWindup, Idle>(outerStateMachine));
        smm.Substates.Add(new SpinWindup(timeManager, smm.Substates, weaponModel));
        smm.Substates.Add(new SpinActive(timeManager, smm.Substates, weaponModel));
    }
}
