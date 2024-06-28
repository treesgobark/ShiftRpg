using ANLG.Utilities.Core.NonStaticUtilities;
using ProjectLoot.Contracts;

namespace ProjectLoot.Components.Interfaces;

public interface IGunComponent
{
    CyclableList<IGunModel> Weapons { get; }
}
