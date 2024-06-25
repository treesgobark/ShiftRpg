using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;

namespace ProjectLoot.Handlers;

public class MeleeWeaponHandler : IUpdateable
{
    private IMeleeWeaponComponent MeleeWeapon { get; }
    private IHitstunComponent? Hitstun { get; }
    private IHitstopComponent? Hitstop { get; }

    public MeleeWeaponHandler(IMeleeWeaponComponent meleeWeapon, IHitstunComponent? hitstun = null, IHitstopComponent? hitstop = null)
    {
        MeleeWeapon = meleeWeapon;
        Hitstun = hitstun;
        Hitstop = hitstop;
    }
    
    public void Activity()
    {
        if (Hitstop is { IsStopped: true } || Hitstun is { IsStunned: true })
        {
            return;
        }
        
        MeleeWeapon.Cache.Activity();
    }
}
