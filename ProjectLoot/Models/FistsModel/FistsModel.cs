using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.DataTypes;

namespace ProjectLoot.Models;

public partial class FistsModel : IMeleeWeaponModel
{
    private readonly ITimeManager _timeManager;
    private StateMachine States { get; }
    
    public FistsModel(IMeleeWeaponComponent meleeWeaponComponent,
                      IEffectsComponent holderEffects, ITimeManager timeManager)
    {
        _timeManager         = timeManager;
        MeleeWeaponComponent = meleeWeaponComponent;
        HolderEffects        = holderEffects;

        States = new StateMachine();
        States.Add(new NotEquipped(States, _timeManager, this));
        States.Add(new Idle(States, _timeManager, this));
        States.Add(new LightRightJab(States, _timeManager, this));
        States.Add(new LightRightJabRecovery(States, _timeManager, this));
        States.Add(new LightLeftJab(States, _timeManager, this));
        States.Add(new LightLeftJabRecovery(States, _timeManager, this));
        States.Add(new LightRightHook(States, _timeManager, this));
        States.Add(new LightRightHookRecovery(States, _timeManager, this));
        States.Add(new LightLeftHook(States, _timeManager, this));
        States.Add(new LightLeftHookRecovery(States, _timeManager, this));
        States.Add(new LightRightFinisher(States, _timeManager, this));
        States.Add(new LightRightFinisherRecovery(States, _timeManager, this));
        States.Add(new HeavyRightJab(States, _timeManager, this));
        States.Add(new HeavyRightJabRecovery(States, _timeManager, this));
        States.Add(new Burst(States, this, timeManager));
        
        States.SetStartingState<NotEquipped>();
    }

    public string WeaponName => MeleeWeaponData.Fists;
    public bool IsEquipped { get; set; }

    public IEffectsComponent HolderEffects { get; }

    public IMeleeWeaponComponent MeleeWeaponComponent { get; }

    private float KnockTowardDistance => 40f;
    
    public void Update()
    {
        States.DoCurrentStateActivity();
    }
    
    public void EvaluateExitConditions()
    {
        States.AdvanceCurrentState();
    }
}