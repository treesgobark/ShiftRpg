using System.Collections.Generic;
using ShiftRpg.Contracts;
using ShiftRpg.Effects;
using ShiftRpg.Effects.Handlers;
using ShiftRpg.InputDevices;

namespace ShiftRpg.Models;

public abstract class ZeroWeapon<T> : IWeapon<T>
{
    public Action<IEffectBundle> ApplyHolderEffects { get; set; } = _ => { };
    public Action<IEffectBundle> ModifyTargetEffects { get; set; } = _ => { };
    public IWeaponHolder Holder { get; set; } = ZeroWeaponHolder.Instance;
    public IEffectBundle TargetHitEffects => EffectBundle.Empty;
    public IEffectBundle HolderHitEffects => EffectBundle.Empty;
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
    public override IGunInputDevice InputDevice => ZeroGunInputDevice.Instance;
}

public class ZeroMeleeWeapon : ZeroWeapon<IMeleeWeaponInputDevice>, IMeleeWeapon
{
    public static readonly ZeroMeleeWeapon Instance = new();
    
    public override IMeleeWeaponInputDevice InputDevice => ZeroMeleeWeaponInputDevice.Instance;
}

public class ZeroWeaponHolder : IWeaponHolder, IEffectReceiver
{
    public static readonly ZeroWeaponHolder Instance = new();
    
    public ZeroWeaponHolder()
    {
        HandlerCollection = new EffectHandlerCollection(this);
    }

    IReadOnlyEffectHandlerCollection IReadOnlyEffectReceiver.HandlerCollection => HandlerCollection;

    public IList<(Guid EffectId, double EffectTime)> RecentEffects { get; } =
        new List<(Guid EffectId, double EffectTime)>();
    public IEffectHandlerCollection HandlerCollection { get; }
    public Team Team => (Team)(-1);
    public IEffectBundle ModifyTargetEffects(IEffectBundle effects) => effects;
    public void SetInputEnabled(bool isEnabled) { }
}
