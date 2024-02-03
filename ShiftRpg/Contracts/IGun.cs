namespace ShiftRpg.Contracts;

public interface IGun : IWeapon
{
    void BeginFire();
    void EndFire();
    
    // void BeginSecondaryFire();
    // void EndSecondaryFire();
    
    void Reload();
}