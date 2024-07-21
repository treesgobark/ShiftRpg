using ANLG.Utilities.Core.States;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.DataTypes;

namespace ProjectLoot.Models;

public partial class FistsModel : IMeleeWeaponModel
{
    private StateMachine States { get; }
    
    public FistsModel(MeleeWeaponData   meleeWeaponData, IMeleeWeaponComponent meleeWeaponComponent,
                      IEffectsComponent holderEffects)
    {
        MeleeWeaponComponent = meleeWeaponComponent;
        HolderEffects        = holderEffects;
        MeleeWeaponData      = meleeWeaponData;

        States = new StateMachine();
        States.Add(new NotEquipped(States, FrbTimeManager.Instance, this));
        States.Add(new Idle(States, FrbTimeManager.Instance, this));
        States.Add(new RightJab(States, FrbTimeManager.Instance, this));
        States.Add(new RightJabRecovery(States, FrbTimeManager.Instance, this));
        States.Add(new LeftJab(States, FrbTimeManager.Instance, this));
        States.Add(new LeftJabRecovery(States, FrbTimeManager.Instance, this));
        States.Add(new RightHook(States, FrbTimeManager.Instance, this));
        States.Add(new RightHookRecovery(States, FrbTimeManager.Instance, this));
        States.Add(new LeftHook(States, FrbTimeManager.Instance, this));
        States.Add(new LeftHookRecovery(States, FrbTimeManager.Instance, this));
        States.Add(new RightFinisher(States, FrbTimeManager.Instance, this));
        States.Add(new RightFinisherRecovery(States, FrbTimeManager.Instance, this));
        
        States.InitializeStartingState<NotEquipped>();
    }
    
    public MeleeWeaponData MeleeWeaponData { get; set; }
    public bool IsEquipped { get; set; }

    private IEffectsComponent HolderEffects { get; }

    private IMeleeWeaponComponent MeleeWeaponComponent { get; }
    
    public void Activity()
    {
        States.DoCurrentStateActivity();
    }
}