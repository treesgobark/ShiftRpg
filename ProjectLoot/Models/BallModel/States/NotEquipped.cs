using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using ProjectLoot.Contracts;
using ProjectLoot.Controllers.ModularStates;

namespace ProjectLoot.Models.BallModel;

public class NotEquipped : ModularDelegateState
{
    public NotEquipped(IReadonlyStateMachine states, IMeleeWeaponModel weaponModel)
    {
        AddExitCondition(() => 
        {
            if (weaponModel.IsEquipped)
            {
                return states.Get<Idle>();
            }

            return null;
        });
    }
}
