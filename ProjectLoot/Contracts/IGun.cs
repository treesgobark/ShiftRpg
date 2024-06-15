namespace ProjectLoot.Contracts;

public interface IGun : IWeapon<IGunInputDevice>
{
    int MagazineRemaining { get; }
    int MagazineSize { get; }
    TimeSpan TimePerRound { get; }
    TimeSpan ReloadTime { get; }
    FiringType FiringType { get; }
    
    void Fire();
    void StartReload();
    void FillMagazine();
}

public enum FiringType
{
    Semiautomatic,
    Automatic,
}