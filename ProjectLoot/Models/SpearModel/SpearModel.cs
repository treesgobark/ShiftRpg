using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.DataTypes;

namespace ProjectLoot.Models.SpearModel;

public partial class SpearModel : IMeleeWeaponModel
{
    private StateMachine States { get; }
    
    public SpearModel(IMeleeWeaponComponent meleeWeaponComponent,
                      IEffectsComponent holderEffects, ITimeManager timeManager)
    {
        MeleeWeaponComponent = meleeWeaponComponent;
        HolderEffects        = holderEffects;

        States = new StateMachine();
        States.Add(new NotEquipped(States, timeManager, this));
        States.Add(new Idle(States, timeManager, this));
        States.Add(new Thrust(States, timeManager, this));
        States.Add(new Toss(States, timeManager, this));
        
        States.SetStartingState<NotEquipped>();
    }
    
    public string WeaponName => MeleeWeaponData.Spear;
    public bool IsEquipped { get; set; }
    
    public IEffectsComponent HolderEffects { get; }
    public IMeleeWeaponComponent MeleeWeaponComponent { get; }
    
    public void Update()
    {
        States.DoCurrentStateActivity();
    }
    
    public void EvaluateExitConditions()
    {
        States.AdvanceCurrentState();
    }
}