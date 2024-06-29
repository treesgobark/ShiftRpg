using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using ProjectLoot.Contracts;

namespace ProjectLoot.Components;

public partial class MeleeWeaponComponent
{
    private StateMachine StateMachine { get; }
    
    public MeleeWeaponComponent()
    {
        // StateMachine = new StateMachine();
        // StateMachine.Add(new Idle(this, StateMachine, FrbTimeManager.Instance));
        // StateMachine.Add(new Startup(this, StateMachine, FrbTimeManager.Instance));
        // StateMachine.Add(new Active(this, StateMachine, FrbTimeManager.Instance));
        // StateMachine.Add(new Recovery(this, StateMachine, FrbTimeManager.Instance));
        // StateMachine.InitializeStartingState<Idle>();
    }
    
    private CyclableList<IMeleeWeaponModel> MeleeWeapons { get; } = [];

    public IMeleeWeaponModel CurrentMeleeWeapon => MeleeWeapons.CurrentItem;
    public bool IsEmpty => MeleeWeapons.Count == 0;

    public void Equip(IMeleeWeaponInputDevice inputDevice)
    {
    }

    public void Unequip()
    {
    }

    public void CycleToNextWeapon()
    {
        IMeleeWeaponModel previousGun = CurrentMeleeWeapon;
        MeleeWeapons.CycleToNextItem();
        
        if (previousGun == CurrentMeleeWeapon)
        {
            return;
        }
    }

    public void CycleToPreviousWeapon()
    {
        IMeleeWeaponModel previousGun = CurrentMeleeWeapon;
        MeleeWeapons.CycleToPreviousItem();
        
        if (previousGun == CurrentMeleeWeapon)
        {
            return;
        }
    }
}