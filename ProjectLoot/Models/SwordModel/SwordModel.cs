using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.DataTypes;

namespace ProjectLoot.Models.SwordModel;

public class SwordModel : IMeleeWeaponModel
{
    private readonly ITimeManager _timeManager;
    private StateMachine States { get; }
    
    public SwordModel(MeleeWeaponData   meleeWeaponData, IMeleeWeaponComponent meleeWeaponComponent,
                      IEffectsComponent holderEffects,   ITimeManager          timeManager)
    {
        _timeManager         = timeManager;
        MeleeWeaponComponent = meleeWeaponComponent;
        HolderEffects        = holderEffects;
        MeleeWeaponData      = meleeWeaponData;

        States = new StateMachine();
        States.Add(new NotEquipped(States, _timeManager, this));
        States.Add(new Idle(States, _timeManager, this));
        States.Add(new Slash1(States, _timeManager, this));
        States.Add(new Slash1Recovery(States, _timeManager, this));
        States.Add(new Slash2(States, _timeManager, this));
        States.Add(new Slash2Recovery(States, _timeManager, this));
        States.Add(new Slash3(States, _timeManager, this));
        States.Add(new Slash3Recovery(States, _timeManager, this));
        States.Add(new CircleSlash(States, _timeManager, this));
        States.Add(new CircleSlashRecovery(States, _timeManager, this));
        States.Add(new Spin(_timeManager, States, this));
        
        States.SetStartingState<NotEquipped>();
    }
    
    public MeleeWeaponData MeleeWeaponData { get; set; }
    public bool IsEquipped { get; set; }

    public IEffectsComponent HolderEffects { get; }

    public IMeleeWeaponComponent MeleeWeaponComponent { get; }
    
    public void Activity()
    {
        States.DoCurrentStateActivity();
    }
    
    public void EvaluateExitConditions()
    {
        States.AdvanceCurrentState();
    }
}