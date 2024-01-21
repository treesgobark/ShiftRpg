namespace ShiftRpg.Contracts;

public interface IGun
{
    void BeginFire();
    void EndFire();
    
    // void BeginSecondaryFire();
    // void EndSecondaryFire();
    
    void Reload();
}