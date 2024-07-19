using ANLG.Utilities.Core.States;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.DataTypes;

namespace ProjectLoot.Models;

public partial class FistsModel : IMeleeWeaponModel
{
    private StateMachine StateMachine { get; }
    
    public FistsModel(MeleeWeaponData   meleeWeaponData, IMeleeWeaponComponent meleeWeaponComponent,
                      IEffectsComponent holderEffects)
    {
        MeleeWeaponComponent = meleeWeaponComponent;
        HolderEffects        = holderEffects;
        MeleeWeaponData      = meleeWeaponData;

        StateMachine = new StateMachine();
        StateMachine.Add(new NotEquipped(StateMachine, FrbTimeManager.Instance, this));
        StateMachine.Add(new Idle(StateMachine, FrbTimeManager.Instance, this));
        StateMachine.Add(new RightJab(StateMachine, FrbTimeManager.Instance, this));
        StateMachine.Add(new RightJabRecovery(StateMachine, FrbTimeManager.Instance, this));
        StateMachine.Add(new LeftJab(StateMachine, FrbTimeManager.Instance, this));
        StateMachine.Add(new LeftJabRecovery(StateMachine, FrbTimeManager.Instance, this));
        StateMachine.Add(new RightHook(StateMachine, FrbTimeManager.Instance, this));
        StateMachine.Add(new RightHookRecovery(StateMachine, FrbTimeManager.Instance, this));
        StateMachine.Add(new LeftHook(StateMachine, FrbTimeManager.Instance, this));
        StateMachine.Add(new LeftHookRecovery(StateMachine, FrbTimeManager.Instance, this));
        StateMachine.Add(new RightFinisher(StateMachine, FrbTimeManager.Instance, this));
        StateMachine.Add(new RightFinisherRecovery(StateMachine, FrbTimeManager.Instance, this));
        
        StateMachine.InitializeStartingState<NotEquipped>();
    }
    
    public MeleeWeaponData MeleeWeaponData { get; set; }
    public bool IsEquipped { get; set; }

    private IEffectsComponent HolderEffects { get; }

    private IMeleeWeaponComponent MeleeWeaponComponent { get; }
    
    public void Activity()
    {
        StateMachine.DoCurrentStateActivity();
    }
}