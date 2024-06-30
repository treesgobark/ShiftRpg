using ANLG.Utilities.Core.States;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.DataTypes;
using ProjectLoot.ViewModels;

namespace ProjectLoot.Models;

public partial class StandardGunModel : IGunModel
{
    private StateMachine StateMachine { get; } = new();
    
    public StandardGunModel(GunData gunData, IGunComponent gunComponent, IGunViewModel gunViewModel)
    {
        GunData                 = gunData;
        GunComponent            = gunComponent;
        GunViewModel            = gunViewModel;
        CurrentRoundsInMagazine = gunData.MagazineSize;
        
        StateMachine.Add(new NotEquipped(StateMachine, FrbTimeManager.Instance, this));
        StateMachine.Add(new Ready(StateMachine, FrbTimeManager.Instance, this));
        StateMachine.Add(new Recovery(StateMachine, FrbTimeManager.Instance, this));
        StateMachine.Add(new Reloading(StateMachine, FrbTimeManager.Instance, this));
        StateMachine.InitializeStartingState<NotEquipped>();
    }
    
    public GunData GunData { get; }
    private IGunComponent GunComponent { get; }
    private IGunViewModel GunViewModel { get; }
    public int CurrentRoundsInMagazine { get; set; }
    public bool IsEquipped { get; set; } = false;
    public bool IsFull => CurrentRoundsInMagazine  == GunData.MagazineSize;
    public bool IsEmpty => CurrentRoundsInMagazine == 0;
    
    public void Activity()
    {
        StateMachine.DoCurrentStateActivity();
    }
}