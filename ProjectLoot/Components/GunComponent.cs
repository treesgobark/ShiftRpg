using ANLG.Utilities.Core.NonStaticUtilities;
using FlatRedBall;
using FlatRedBall.Forms.MVVM;
using Microsoft.Xna.Framework;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.DataTypes;
using ProjectLoot.Effects;
using ProjectLoot.InputDevices;
using ProjectLoot.ViewModels;

namespace ProjectLoot.Components;

public class GunComponent : ViewModel, IGunViewModel, IGunComponent
{
    public Sprite GunSprite { get; }
    public Vector3 GunPosition => GunSprite.Position;
    public Rotation GunRotation => Rotation.FromRadians(GunSprite.RotationZ);
    public Team Team { get; }
    public IGunInputDevice GunInputDevice => InputDevice.GunInputDevice;
    
    private IGameplayInputDevice InputDevice { get; }
    private CyclableList<IGunModel> Guns { get; } = [];
    
    public GunComponent(Sprite gunSprite, Team team, IGameplayInputDevice inputDevice)
    {
        GunSprite   = gunSprite;
        Team        = team;
        InputDevice = inputDevice;
    }

    public bool IsEmpty => Guns.Count == 0;
    
    public void Add(IGunModel gunModel) => Guns.Add(gunModel);

    public void CycleToNextWeapon()
    {
        if (!Guns.TryGetCurrentItem(out IGunModel gunModel))
        {
            throw new InvalidOperationException("No guns present. Cannot cycle.");
        }

        gunModel.IsEquipped = false;
        IGunModel nextGun = Guns.CycleToNextItem();
        nextGun.IsEquipped         = true;
        GunSprite.CurrentChainName = nextGun.GunData.GunName;
    }

    public void CycleToPreviousWeapon()
    {
        if (!Guns.TryGetCurrentItem(out IGunModel gunModel))
        {
            throw new InvalidOperationException("No guns present. Cannot cycle.");
        }

        gunModel.IsEquipped = false;
        IGunModel nextGun = Guns.CycleToPreviousItem();
        nextGun.IsEquipped         = true;
        GunSprite.CurrentChainName = nextGun.GunData.GunName;
    }

    public void Equip()
    {
        if (!Guns.TryGetCurrentItem(out IGunModel gunModel))
        {
            throw new InvalidOperationException("No guns present. Cannot equip.");
        }

        IsEquipped = true;

        gunModel.IsEquipped = true;
        
        GunSprite.Visible          = true;
        GunSprite.CurrentChainName = gunModel.GunData.GunName;
    }

    public void Unequip()
    {
        if (Guns.TryGetCurrentItem(out IGunModel gunModel))
        {
            gunModel.IsEquipped = false;
        }

        IsEquipped = false;

        GunSprite.Visible = false;
    }

    public void Activity()
    {
        foreach (IGunModel gunModel in Guns)
        {
            gunModel.Activity();
        }

        SetSpriteFlip();
    }

    private void SetSpriteFlip()
    {
        Rotation rotation = Rotation.FromRadians(GunSprite.RotationZ);
        
        if (rotation.CondensedDegrees is > -90 and < 90)
        {
            GunSprite.FlipVertical = false;
        }
        else
        {
            GunSprite.FlipVertical = true;
        }
    }

    public int CurrentMagazineCount
    {
        get => Get<int>();
        set => Set(value);
    }

    public int MaximumMagazineCount
    {
        get => Get<int>();
        set => Set(value);
    }

    public GunClass GunClass
    {
        get => Get<GunClass>();
        set => Set(value);
    }

    public bool IsEquipped
    {
        get => Get<bool>();
        set => Set(value);
    }

    public event Action<int>? GunFired;

    public void PublishGunFiredEvent(int ammoUsed)
    {
        GunFired?.Invoke(ammoUsed);
        CurrentMagazineCount -= ammoUsed;
    }
}
