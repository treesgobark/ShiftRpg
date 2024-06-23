using FlatRedBall;
using FlatRedBall.Graphics;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.Effects;
using ProjectLoot.Entities;
using ProjectLoot.InputDevices;
using ProjectLoot.Models;

namespace ProjectLoot.Components;

public class WeaponsComponent : IWeaponsComponent, IDestroyable
{
    public WeaponsComponent(IGameplayInputDevice gameplayInputDevice, Team team, PositionedObject parent, IWeaponHolder holder)
    {
        DefaultGun gun = new()
        {
            RelativeX = 10
        };

        gun.AttachTo(parent);
        gun.Effects.Team = team;
        gun.Holder = holder;

        GunCache = new WeaponCache<IGun, IGunInputDevice>(ZeroGun.Instance, new GunInputDevice(gameplayInputDevice));
        GunCache.Add(gun);

        DefaultSword melee = new();

        melee.AttachTo(parent);
        melee.Effects.Team = team;
        melee.Holder = holder;

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