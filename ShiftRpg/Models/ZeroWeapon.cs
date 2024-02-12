using System.Collections.Generic;
using ANLG.Utilities.FlatRedBall.Controllers;
using ShiftRpg.Contracts;
using ShiftRpg.Controllers.Gun;
using ShiftRpg.Effects;
using ShiftRpg.Entities;
using ShiftRpg.InputDevices;

namespace ShiftRpg.Models;

public abstract class ZeroWeapon<T> : IWeapon<T>
{
    public Action<IReadOnlyList<IEffect>> ApplyHolderEffects { get; set; } = _ => { };
    public Action<IReadOnlyList<IEffect>> ModifyTargetEffects { get; set; } = _ => { };
    public IReadOnlyList<IEffect> TargetHitEffects => IEffect.EmptyList;
    public IReadOnlyList<IEffect> HolderHitEffects => IEffect.EmptyList;
    public Team Team { get; set; } = (Team)(-1);
    public SourceTag Source { get; set; } = (SourceTag)(-1);

    public virtual T InputDevice { get; set; }
    
    public void Equip(T inputDevice) { }
    public void Unequip() { }
}

public class ZeroGun : ZeroWeapon<IGunInputDevice>, IGun
{
    public static readonly ZeroGun Instance = new();
    
    public int MagazineRemaining => 0;
    public int MagazineSize => 0;
    public TimeSpan TimePerRound => TimeSpan.Zero;
    public TimeSpan ReloadTime => TimeSpan.Zero;
    public FiringType FiringType { get; } = (FiringType)(-1);

    public void Fire() { }
    public void StartReload() { }
    public void FillMagazine() { }

    public ControllerCollection<IGun, GunController> Controllers { get; }
    public override IGunInputDevice InputDevice => ZeroGunInputDevice.Instance;
}

public class ZeroMeleeWeapon : ZeroWeapon<IMeleeWeaponInputDevice>, IMeleeWeapon
{
    public static readonly ZeroMeleeWeapon Instance = new();

    public ControllerCollection<IGun, GunController> Controllers { get; }
    public override IMeleeWeaponInputDevice InputDevice => ZeroMeleeWeaponInputDevice.Instance;
}