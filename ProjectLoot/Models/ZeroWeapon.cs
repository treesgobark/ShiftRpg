using ProjectLoot.Components;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.DataTypes;
using ProjectLoot.Effects;
using ProjectLoot.Entities;
using ProjectLoot.InputDevices;

namespace ProjectLoot.Models;

public abstract class ZeroWeapon<T> : IWeapon<T>
{
    public Action<IEffectBundle> ApplyHolderEffects { get; set; } = _ => { };
    public Action<IEffectBundle> ModifyTargetEffects { get; set; } = _ => { };
    public IWeaponHolder Holder { get; set; } = ZeroWeaponHolder.Instance;
    public IEffectBundle TargetHitEffects => EffectBundle.Empty;
    public IEffectBundle HolderHitEffects => EffectBundle.Empty;
    public Team Team { get; set; } = (Team)(-1);
    public SourceTag Source { get; set; } = (SourceTag)(-1);

    public IEffectsComponent Effects { get; } = new EffectsComponent { Team = (Team)(-1) };
    public virtual T InputDevice { get; set; }
    
    public void Equip(T inputDevice) { }
    public void Unequip() { }
    public void Activity() { }

    public void Destroy() { }
}

public class ZeroGun : ZeroWeapon<IGunInputDevice>, IGun
{
    public static readonly ZeroGun Instance = new();
    
    public int MagazineRemaining => 0;
    public int MagazineSize => 0;
    public TimeSpan TimePerRound => TimeSpan.Zero;
    public TimeSpan ReloadTime => TimeSpan.Zero;
    public FiringType FiringType { get; } = (FiringType)(-1);
    public float ReloadProgress { get; set; }

    public void Fire() { }
    public void StartReload() { }
    public void FillMagazine() { }
    public override IGunInputDevice InputDevice => ZeroGunInputDevice.Instance;
}

public class ZeroMeleeWeapon : ZeroWeapon<IMeleeWeaponInputDevice>, IMeleeWeapon
{
    public static readonly ZeroMeleeWeapon Instance = new();
    
    public override IMeleeWeaponInputDevice InputDevice => ZeroMeleeWeaponInputDevice.Instance;
    public bool IsActive { get; set; } = false;
    public AttackData CurrentAttackData { get; }
    public void ShowHitbox(bool shouldShow) { }

    public MeleeHitbox SpawnHitbox()
    {
        return null;
    }
}

public class ZeroWeaponHolder : IWeaponHolder
{
    public static readonly ZeroWeaponHolder Instance = new();

    public void SetInputEnabled(bool isEnabled) { }

    public IEffectBundle ModifyTargetEffects(IEffectBundle effects) => effects;
    public IEffectsComponent Effects { get; set; } = new EffectsComponent { Team = (Team)(-1) };
    public bool InputEnabled { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public float XVelocity { get; set; }
    public float YVelocity { get; set; }
    public float ZVelocity { get; set; }
    public float XAcceleration { get; set; }
    public float YAcceleration { get; set; }
    public float ZAcceleration { get; set; }
}
