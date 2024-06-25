using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.Models;

namespace ProjectLoot.Components;

public class GunComponent : IGunComponent
{
    public GunComponent(IGunInputDevice gunInputDevice)
    {
        Cache = new WeaponCache<IGun, IGunInputDevice>(gunInputDevice);
    }
    
    public IWeaponCache<IGun, IGunInputDevice> Cache { get; }
}