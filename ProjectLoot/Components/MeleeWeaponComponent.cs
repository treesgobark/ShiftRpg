using ANLG.Utilities.Core;
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
    private Sprite HolderSprite { get; }
    private PositionedObject HolderGameplayCenter { get; }
    public Team Team { get; }

    public Vector3 AttackOrigin => Origin.Position;
    public Vector3 HolderSpritePosition => HolderSprite.Position;
    public Vector3 HolderGameplayCenterPosition => HolderGameplayCenter.Position;
    public Rotation AttackDirection => Rotation.FromRadians(Origin.RotationZ);
    public Vector3 HolderVelocity => Holder.Velocity;

    public MeleeWeaponComponent(Team             team,   IGameplayInputDevice inputDevice,       PositionedObject holder,
                                PositionedObject origin, Sprite               meleeWeaponSprite, Sprite           holderSprite, PositionedObject holderGameplayCenter)
    {
        Holder                    = holder;
        Origin                    = origin;
        MeleeWeaponSprite         = meleeWeaponSprite;
        HolderSprite              = holderSprite;
        HolderGameplayCenter = holderGameplayCenter;
        Team                      = team;
        InputDevice               = inputDevice;
    }

    public IMeleeWeaponInputDevice MeleeWeaponInputDevice => InputDevice.MeleeWeaponInputDevice;
    
    private CyclableList<IMeleeWeaponModel> MeleeWeapons { get; } = [];
    
    public void Add(IMeleeWeaponModel meleeWeapon) => MeleeWeapons.Add(meleeWeapon);

    public IMeleeWeaponModel CurrentMeleeWeapon => MeleeWeapons.CurrentItem;
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
        MeleeWeaponSprite.CurrentChainName = nextMeleeWeapon.WeaponName;
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
        MeleeWeaponSprite.CurrentChainName = nextMeleeWeapon.WeaponName;
    }

    public void Equip()
    {
        if (!MeleeWeapons.TryGetCurrentItem(out IMeleeWeaponModel meleeWeaponModel))
        {
            throw new InvalidOperationException("No melee weapons present. Cannot equip.");
        }

        meleeWeaponModel.IsEquipped = true;
        
        MeleeWeaponSprite.Visible          = false;
        MeleeWeaponSprite.CurrentChainName = meleeWeaponModel.WeaponName;
    }

    public void Unequip()
    {
        if (MeleeWeapons.TryGetCurrentItem(out IMeleeWeaponModel meleeWeaponModel))
        {
            meleeWeaponModel.IsEquipped = false;
        }

        MeleeWeaponSprite.Visible = false;
    }

    public void Activity()
    {
        foreach (IMeleeWeaponModel meleeWeapon in MeleeWeapons)
        {
            meleeWeapon.Update();
        }
    }
    
    public void AttachObjectToAttackOrigin(PositionedObject obj)
    {
        obj.AttachTo(Origin);
    }
}