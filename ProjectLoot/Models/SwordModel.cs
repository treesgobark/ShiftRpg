using ANLG.Utilities.Core.States;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.DataTypes;

namespace ProjectLoot.Models;

public partial class SwordModel : IMeleeWeaponModel
{
    private StateMachine StateMachine { get; }
    
    public SwordModel(MeleeWeaponData   meleeWeaponData, IMeleeWeaponComponent meleeWeaponComponent,
                      IEffectsComponent holderEffects)
    {
        MeleeWeaponComponent = meleeWeaponComponent;
        HolderEffects        = holderEffects;
        MeleeWeaponData = meleeWeaponData;
        CurrentAttackData    = GlobalContent.AttackData[AttackData.DefaultSwordSlash];

        StateMachine = new StateMachine();
        StateMachine.Add(new NotEquipped(StateMachine, FrbTimeManager.Instance, this));
        StateMachine.Add(new Idle(StateMachine, FrbTimeManager.Instance, this));
        StateMachine.Add(new Slash1(StateMachine, FrbTimeManager.Instance, this));
        StateMachine.Add(new Slash1Recovery(StateMachine, FrbTimeManager.Instance, this));
        StateMachine.Add(new Slash2(StateMachine, FrbTimeManager.Instance, this));
        StateMachine.Add(new Slash2Recovery(StateMachine, FrbTimeManager.Instance, this));
        StateMachine.Add(new Slash3(StateMachine, FrbTimeManager.Instance, this));
        StateMachine.Add(new Slash3Recovery(StateMachine, FrbTimeManager.Instance, this));
        StateMachine.Add(new CircleSlash(StateMachine, FrbTimeManager.Instance, this));
        StateMachine.Add(new CircleSlashRecovery(StateMachine, FrbTimeManager.Instance, this));
        
        StateMachine.InitializeStartingState<NotEquipped>();
    }
    
    public MeleeWeaponData MeleeWeaponData { get; set; }
    public bool IsEquipped { get; set; }

    private AttackData CurrentAttackData { get; set; }
    private IEffectsComponent HolderEffects { get; }

    private IMeleeWeaponComponent MeleeWeaponComponent { get; }
    
    public void Activity()
    {
        StateMachine.DoCurrentStateActivity();
    }
}