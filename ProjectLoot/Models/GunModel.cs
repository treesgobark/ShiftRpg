using ProjectLoot.Contracts;
using ProjectLoot.DataTypes;

namespace ProjectLoot.Models;

public class GunModel : IGunModel
{
    public GunModel(GunData gunData)
    {
        GunData                 = gunData;
        CurrentRoundsInMagazine = gunData.MagazineSize;
    }
    
    public GunData GunData { get; }
    public int CurrentRoundsInMagazine { get; set; }
}