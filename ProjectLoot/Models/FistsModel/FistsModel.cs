using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.DataTypes;

namespace ProjectLoot.Models;

public partial class FistsModel : IMeleeWeaponModel
{
    private readonly ITimeManager _timeManager;
    private StateMachine States { get; }
    
    public FistsModel(MeleeWeaponData   meleeWeaponData, IMeleeWeaponComponent meleeWeaponComponent,
                      IEffectsComponent holderEffects, ITimeManager timeManager)
    {
        _timeManager         = timeManager;
        MeleeWeaponComponent = meleeWeaponComponent;
        HolderEffects        = holderEffects;
        MeleeWeaponData      = meleeWeaponData;

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
        
        States.InitializeStartingState<NotEquipped>();
    }
    
    public MeleeWeaponData MeleeWeaponData { get; set; }
    public bool IsEquipped { get; set; }

    private IEffectsComponent HolderEffects { get; }

    private IMeleeWeaponComponent MeleeWeaponComponent { get; }

    private TimeSpan KnockTowardDuration => TimeSpan.FromMilliseconds(60);
    private float KnockTowardDistance => 40f;
    
    public void Activity()
    {
        States.DoCurrentStateActivity();
    }
    
    public void EvaluateExitConditions()
    {
        States.AdvanceCurrentState();
    }
}