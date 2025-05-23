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
        States.Add(new LightRightJab(States, FrbTimeManager.Instance, this));
        States.Add(new LightRightJabRecovery(States, FrbTimeManager.Instance, this));
        States.Add(new LightLeftJab(States, FrbTimeManager.Instance, this));
        States.Add(new LightLeftJabRecovery(States, FrbTimeManager.Instance, this));
        States.Add(new LightRightHook(States, FrbTimeManager.Instance, this));
        States.Add(new LightRightHookRecovery(States, FrbTimeManager.Instance, this));
        States.Add(new LightLeftHook(States, FrbTimeManager.Instance, this));
        States.Add(new LightLeftHookRecovery(States, FrbTimeManager.Instance, this));
        States.Add(new LightRightFinisher(States, FrbTimeManager.Instance, this));
        States.Add(new LightRightFinisherRecovery(States, FrbTimeManager.Instance, this));
        States.Add(new HeavyRightJab(States, FrbTimeManager.Instance, this));
        
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
    
    public void EvaluateExitConditions()
    {
        States.EvaluateExitConditions();
    }
}