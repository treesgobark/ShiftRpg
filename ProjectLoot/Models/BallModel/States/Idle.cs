using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using ProjectLoot.Contracts;
using ProjectLoot.Controllers.ModularStates;

namespace ProjectLoot.Models.BallModel;

public class Idle : ModularDelegateState
{
    public Idle(IReadonlyStateMachine states, IMeleeWeaponModel weaponModel)
    {
        AddExitCondition(() => 
        {
            if (!weaponModel.IsEquipped)
            {
                return states.Get<NotEquipped>();
            }

            // if (weaponModel.MeleeWeaponComponent.MeleeWeaponInputDevice.LightAttack.WasJustPressed)
            // {
            //     return states.Get<LightRightJab>();
            // }
            //
            // if (weaponModel.MeleeWeaponComponent.MeleeWeaponInputDevice.HeavyAttack.WasJustPressed)
            // {
            //     return states.Get<HeavyRightJab>();
            // }

            return null;
        });
    }
}
