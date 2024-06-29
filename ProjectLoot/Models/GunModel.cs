using FlatRedBall.Forms.MVVM;
using ProjectLoot.Contracts;
using ProjectLoot.DataTypes;

namespace ProjectLoot.Models;

public class GunModel : ViewModel, IGunModel
{
    public GunModel(GunData gunData)
    {
        GunData                 = gunData;
        CurrentRoundsInMagazine = gunData.MagazineSize;
    }
    
    public GunData GunData { get; }
    public int CurrentRoundsInMagazine { get => Get<int>(); set => Set(value); }
    public bool IsFull => CurrentRoundsInMagazine  == GunData.MagazineSize;
    public bool IsEmpty => CurrentRoundsInMagazine == 0;
}