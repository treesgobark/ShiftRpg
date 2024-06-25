using ProjectLoot.Contracts;

namespace ProjectLoot.Components.Interfaces;

public interface IGunComponent
{
    IWeaponCache<IGun, IGunInputDevice> Cache { get; }
}
