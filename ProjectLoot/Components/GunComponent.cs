using ANLG.Utilities.Core.NonStaticUtilities;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.Models;

namespace ProjectLoot.Components;

public class GunComponent : IGunComponent
{
    public CyclableList<IGunModel> Weapons { get; } = [];
}