using ANLG.Utilities.Core.NonStaticUtilities;
using FlatRedBall;
using Microsoft.Xna.Framework;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.Effects;

namespace ProjectLoot.Components;

public class MeleeWeaponComponent : IMeleeWeaponComponent
{
    private IGameplayInputDevice InputDevice { get; }
    private PositionedObject Holder { get; }
    private PositionedObject Origin { get; }
    private Sprite MeleeWeaponSprite { get; }
    public Team Team { get; }

    public Vector3 AttackOrigin => Origin.Position;
    public Rotation AttackDirection => Rotation.FromRadians(Origin.RotationZ);
    public Vector3 HolderVelocity => Holder.Velocity;

    public MeleeWeaponComponent(Team team, IGameplayInputDevice inputDevice, PositionedObject holder, PositionedObject origin, Sprite meleeWeaponSprite)
    {
        Holder            = holder;
        Origin            = origin;
        MeleeWeaponSprite = meleeWeaponSprite;
        Team              = team;
        InputDevice       = inputDevice;
    }

    public IMeleeWeaponInputDevice MeleeWeaponInputDevice => InputDevice.MeleeWeaponInputDevice;
    
    private CyclableList<IMeleeWeaponModel> MeleeWeapons { get; } = [];
    
    public void Add(IMeleeWeaponModel meleeWeapon) => MeleeWeapons.Add(meleeWeapon);

    private IMeleeWeaponModel CurrentMeleeWeapon => MeleeWeapons.CurrentItem;
    public bool IsEmpty => MeleeWeapons.Count == 0;

    public void CycleToNextWeapon()
    {
        if (!MeleeWeapons.TryGetCurrentItem(out IMeleeWeaponModel meleeWeaponModel))
        {
            throw new InvalidOperationException("No melee weapons present. Cannot cycle.");
        }

        meleeWeaponModel.IsEquipped = false;
        
        IMeleeWeaponModel nextMeleeWeapon = MeleeWeapons.CycleToNextItem();
        
        nextMeleeWeapon.IsEquipped         = true;
        MeleeWeaponSprite.CurrentChainName = nextMeleeWeapon.MeleeWeaponData.Name;
    }

    public void CycleToPreviousWeapon()
    {
        if (!MeleeWeapons.TryGetCurrentItem(out IMeleeWeaponModel meleeWeaponModel))
        {
            throw new InvalidOperationException("No melee weapons present. Cannot cycle.");
        }

        meleeWeaponModel.IsEquipped = false;
        
        IMeleeWeaponModel nextMeleeWeapon = MeleeWeapons.CycleToPreviousItem();
        
        nextMeleeWeapon.IsEquipped         = true;
        MeleeWeaponSprite.CurrentChainName = nextMeleeWeapon.MeleeWeaponData.Name;
    }

    public void Equip()
    {
        if (!MeleeWeapons.TryGetCurrentItem(out IMeleeWeaponModel meleeWeaponModel))
        {
            throw new InvalidOperationException("No melee weapons present. Cannot equip.");
        }

        // IsEquipped = true;

        meleeWeaponModel.IsEquipped = true;
        
        // MeleeWeaponSprite.Visible          = true;
        MeleeWeaponSprite.CurrentChainName = meleeWeaponModel.MeleeWeaponData.Name;
    }

    public void Unequip()
    {
        if (MeleeWeapons.TryGetCurrentItem(out IMeleeWeaponModel meleeWeaponModel))
        {
            meleeWeaponModel.IsEquipped = false;
        }

        // IsEquipped = false;

        MeleeWeaponSprite.Visible = false;
    }

    public void Activity()
    {
        foreach (IMeleeWeaponModel meleeWeapon in MeleeWeapons)
        {
            meleeWeapon.Activity();
        }
    }
    
    public void AttachObjectToAttackOrigin(PositionedObject obj)
    {
        obj.AttachTo(Origin);
    }
}