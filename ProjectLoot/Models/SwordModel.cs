using ANLG.Utilities.Core.States;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.DataTypes;

namespace ProjectLoot.Models;

public partial class SwordModel : IMeleeWeaponModel
{
    private StateMachine States { get; }
    
    public SwordModel(MeleeWeaponData   meleeWeaponData, IMeleeWeaponComponent meleeWeaponComponent,
                      IEffectsComponent holderEffects)
    {
        MeleeWeaponComponent = meleeWeaponComponent;
        HolderEffects        = holderEffects;
        MeleeWeaponData = meleeWeaponData;

        States = new StateMachine();
        States.Add(new NotEquipped(States, FrbTimeManager.Instance, this));
        States.Add(new Idle(States, FrbTimeManager.Instance, this));
        States.Add(new Slash1(States, FrbTimeManager.Instance, this));
        States.Add(new Slash1Recovery(States, FrbTimeManager.Instance, this));
        States.Add(new Slash2(States, FrbTimeManager.Instance, this));
        States.Add(new Slash2Recovery(States, FrbTimeManager.Instance, this));
        States.Add(new Slash3(States, FrbTimeManager.Instance, this));
        States.Add(new Slash3Recovery(States, FrbTimeManager.Instance, this));
        States.Add(new CircleSlash(States, FrbTimeManager.Instance, this));
        States.Add(new CircleSlashRecovery(States, FrbTimeManager.Instance, this));
        
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