using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;

namespace ProjectLoot.Handlers;

public class GunHandler : IUpdateable
{
    private IGunComponent Gun { get; }
    private IHitstunComponent? Hitstun { get; }
    private IHitstopComponent? Hitstop { get; }

    public GunHandler(IGunComponent gun, IHitstunComponent? hitstun = null, IHitstopComponent? hitstop = null)
    {
        Gun = gun;
        Hitstun = hitstun;
        Hitstop = hitstop;
    }
    
    public void Activity()
    {
        if (Hitstop is { IsStopped: true } || Hitstun is { IsStunned: true })
        {
            return;
        }
        
        Gun.Cache.Activity();
    }
}
