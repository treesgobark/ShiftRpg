using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.DataTypes;
using ProjectLoot.Entities;

namespace ProjectLoot.Models.SpearModel;

public partial class SpearModel : IMeleeWeaponModel
{
    private StateMachine States { get; }
    
    public SpearModel(MeleeWeaponData   meleeWeaponData, IMeleeWeaponComponent meleeWeaponComponent,
                      IEffectsComponent holderEffects)
    {
        MeleeWeaponComponent = meleeWeaponComponent;
        HolderEffects        = holderEffects;
        MeleeWeaponData      = meleeWeaponData;

        States = new StateMachine();
        States.Add(new NotEquipped(States, FrbTimeManager.Instance, this));
        States.Add(new Idle(States, FrbTimeManager.Instance, this));
        States.Add(new Thrust(States, FrbTimeManager.Instance, this));
        States.Add(new TossWindup(States, FrbTimeManager.Instance, this));
        States.Add(new TossActive(States, FrbTimeManager.Instance, this));
        States.Add(new TossedSpearInGround(States, FrbTimeManager.Instance, this));
        
        States.InitializeStartingState<NotEquipped>();
    }
    
    public MeleeWeaponData MeleeWeaponData { get; set; }
    public bool IsEquipped { get; set; }
    
    private MeleeHitbox? Hitbox { get; set; }
    private float ChargeProgress { get; set; }
    private Rotation AttackDirection { get; set; }

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