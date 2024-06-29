using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using FlatRedBall;
using FlatRedBall.Forms.MVVM;
using ProjectLoot.Contracts;
using ProjectLoot.DataTypes;
using ProjectLoot.Effects;
using ProjectLoot.InputDevices;
using ProjectLoot.ViewModels;

namespace ProjectLoot.Components;

public partial class GunComponent : ViewModel, IGunViewModel
{
    private StateMachine StateMachine { get; }
    private Sprite GunSprite { get; }
    private Team Team { get; }
    private IGunInputDevice InputDevice { get; set; } = ZeroGunInputDevice.Instance;
    private CyclableList<IGunModel> Guns { get; } = [];
    
    public GunComponent(Sprite gunSprite, Team team)
    {
        GunSprite = gunSprite;
        Team      = team;
        
        StateMachine = new StateMachine();
        StateMachine.Add(new Ready(this, StateMachine, FrbTimeManager.Instance));
        StateMachine.Add(new Recovery(this, StateMachine, FrbTimeManager.Instance));
        StateMachine.Add(new Reloading(this, StateMachine, FrbTimeManager.Instance));
        StateMachine.InitializeStartingState<Ready>();
    }

    public IGunModel? CurrentGun => Guns.CurrentItem;
    public bool IsEmpty => Guns.Count == 0;
    
    public void Add(IGunModel gunModel) => Guns.Add(gunModel);

    public void CycleToNextWeapon()
    {
        IGunModel previousGun = CurrentGun;
        Guns.CycleToNextItem();
        
        if (previousGun == CurrentGun)
        {
            return;
        }

        MaximumMagazineCount       = CurrentGun.GunData.MagazineSize;
        CurrentMagazineCount       = CurrentGun.CurrentRoundsInMagazine;
        GunClass                   = CurrentGun.GunData.GunClass;
        GunSprite.CurrentChainName = CurrentGun.GunData.GunName;
    }

    public void CycleToPreviousWeapon()
    {
        IGunModel previousGun = CurrentGun;
        Guns.CycleToPreviousItem();
        
        if (previousGun == CurrentGun)
        {
            return;
        }

        MaximumMagazineCount       = CurrentGun.GunData.MagazineSize;
        CurrentMagazineCount       = CurrentGun.CurrentRoundsInMagazine;
        GunClass                   = CurrentGun.GunData.GunClass;
        GunSprite.CurrentChainName = CurrentGun.GunData.GunName;
    }

    public void Equip(IGunInputDevice input)
    {
        InputDevice                = input;
        GunSprite.Visible          = true;
        GunSprite.CurrentChainName = CurrentGun.GunData.GunName;
        
        MaximumMagazineCount       = CurrentGun.GunData.MagazineSize;
        CurrentMagazineCount       = CurrentGun.CurrentRoundsInMagazine;
        GunClass                   = CurrentGun.GunData.GunClass;
    }

    public void Unequip()
    {
        InputDevice       = ZeroGunInputDevice.Instance;
        GunSprite.Visible = false;
    }

    public void Activity()
    {
        StateMachine.DoCurrentStateActivity();
        SetSpriteFlip();
    }

    private void SetSpriteFlip()
    {
        Rotation rotation = Rotation.FromRadians(GunSprite.RotationZ);
        
        if (rotation.CondensedDegrees is > -90 and < 90)
        {
            GunSprite.FlipVertical = false;
        }
        else
        {
            GunSprite.FlipVertical = true;
        }
    }

    public int CurrentMagazineCount
    {
        get => Get<int>();
        set => Set(value);
    }

    public int MaximumMagazineCount
    {
        get => Get<int>();
        set => Set(value);
    }

    public GunClass GunClass
    {
        get => Get<GunClass>();
        set => Set(value);
    }

    public event Action<int>? GunFired;
}
