using System.ComponentModel;
using ProjectLoot.DataTypes;

namespace ProjectLoot.ViewModels;

public interface IGunViewModel : INotifyPropertyChanged
{
    int CurrentMagazineCount { get; set; }
    int MaximumMagazineCount { get; set; }
    GunClass GunClass { get; set; }

    event Action<int>? GunFired;
    void PublishGunFiredEvent(int ammoUsed);
}