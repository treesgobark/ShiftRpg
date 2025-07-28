using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.Controllers.ModularStates;

namespace ProjectLoot.Models;

partial class SwordModel
{
    private class Spin : ModularState
    {
        public Spin(ITimeManager timeManager, IReadonlyStateMachine outerStateMachine, IMeleeWeaponModel weaponModel)
        {
            var smm = AddModule(new StateMachineModule<SpinWindup, Idle>(outerStateMachine));
            smm.Substates.Add(new SpinWindup(timeManager, smm.Substates, weaponModel.MeleeWeaponComponent.MeleeWeaponInputDevice));
            smm.Substates.Add(new SpinActive(timeManager, smm.Substates, weaponModel));
            AddModule(new LoggingModule(nameof(Spin)));
        }
    }
}
