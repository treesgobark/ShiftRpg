using FlatRedBall;
using FlatRedBall.Graphics;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.Effects;
using ProjectLoot.Entities;
using ProjectLoot.Factories;
using ProjectLoot.InputDevices;
using ProjectLoot.Models;

namespace ProjectLoot.Components;

public class WeaponsComponent : IWeaponsComponent, IDestroyable
{
    public WeaponsComponent(IGameplayInputDevice gameplayInputDevice, Team team, PositionedObject parent)
    {
        DefaultGun gun = new()
        {
            RelativeX = 10
        };

        gun.AttachTo(parent);
        gun.Team = team;
        // gun.Holder = this;
        gun.Holder = ZeroWeaponHolder.Instance;

        GunCache = new WeaponCache<IGun, IGunInputDevice>(ZeroGun.Instance, new GunInputDevice(gameplayInputDevice));
        GunCache.Add(gun);

        DefaultSword melee = new DefaultSword();

        melee.AttachTo(parent);
        melee.Team = team;
        // melee.Holder = this;
        melee.Holder = ZeroWeaponHolder.Instance;

        MeleeWeaponCache = new WeaponCache<IMeleeWeapon, IMeleeWeaponInputDevice>(ZeroMeleeWeapon.Instance,
            new MeleeWeaponInputDevice(gameplayInputDevice));
        MeleeWeaponCache.Add(melee);
    }
    
    public IWeaponCache<IGun, IGunInputDevice> GunCache { get; }
    public IWeaponCache<IMeleeWeapon, IMeleeWeaponInputDevice> MeleeWeaponCache { get; }

    public void Activity()
    {
        if (GunCache.IsActive)
        {
            GunCache.CurrentWeapon.Activity();
        }

        if (MeleeWeaponCache.IsActive)
        {
            MeleeWeaponCache.CurrentWeapon.Activity();
        }
    }
    
    public void Destroy()
    {
        GunCache.Destroy();
        MeleeWeaponCache.Destroy();
    }
}