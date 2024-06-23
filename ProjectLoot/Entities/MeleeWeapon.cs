using System.Diagnostics;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using FlatRedBall.Input;
using Microsoft.Xna.Framework;
using ProjectLoot.Components;
using ProjectLoot.Contracts;
using ProjectLoot.DataTypes;
using ProjectLoot.Effects;
using ProjectLoot.Factories;
using ProjectLoot.InputDevices;
using Debugger = FlatRedBall.Debugging.Debugger;

namespace ProjectLoot.Entities;

public abstract partial class MeleeWeapon : IMeleeWeapon
{
    public EffectsComponent Effects { get; private set; }
    
    public readonly CyclableList<string> AttackList = new(AttackData.OrderedList);

    /// <summary>
    /// Initialization logic which is executed only one time for this Entity (unless the Entity is pooled).
    /// This method is called when the Entity is added to managers. Entities which are instantiated but not
    /// added to managers will not have this method called.
    /// </summary>
    private void CustomInitialize()
    {
        AttackList.CycleToPreviousItem();
        CurrentAttackData             = GlobalContent.AttackData[AttackList.CycleToNextItem()];
        ParentRotationChangesRotation = true;
        TargetHitEffects              = EffectBundle.Empty;
        HolderHitEffects              = EffectBundle.Empty;
        Effects = new EffectsComponent { Source = SourceTag.Melee };
    }

    private void CustomActivity()
    {
        if (InputManager.Mouse.ScrollWheelChange > 0)
        {
            CurrentAttackData = GlobalContent.AttackData[AttackList.CycleToNextItem()];
            Debugger.CommandLineWrite($"Switched to {CurrentAttackData.Name}");
        }
        else if (InputManager.Mouse.ScrollWheelChange < 0)
        {
            CurrentAttackData = GlobalContent.AttackData[AttackList.CycleToPreviousItem()];
            Debugger.CommandLineWrite($"Switched to {CurrentAttackData.Name}");
        }
    }

    private void CustomDestroy() { }

    private static void CustomLoadStaticContent(string contentManagerName) { }

    #region IWeapon
    
    public IWeaponHolder Holder { get; set; }
    public IEffectBundle TargetHitEffects { get; set; }
    public IEffectBundle HolderHitEffects { get; set; }
    public IMeleeWeaponInputDevice InputDevice { get; set; }

    public void Equip(IMeleeWeaponInputDevice inputDevice)
    {
        InputDevice = inputDevice;
    }

    public void Unequip()
    {
        InputDevice = ZeroMeleeWeaponInputDevice.Instance;
    }
    
    #endregion
    
    #region IMeleeWeapon
    
    public bool IsActive { get; set; } = false;
    public AttackData CurrentAttackData { get; set; }

    public MeleeHitbox SpawnHitbox()
    {
        var hitbox = MeleeHitboxFactory.CreateNew();
        hitbox.RelativePosition = CurrentAttackData.HitboxOffset.ToVector3();
        
        float leftX   = CurrentAttackData.HitboxSizeX / -2;
        float rightX  = CurrentAttackData.HitboxSizeX /  2;
        float topY    = CurrentAttackData.HitboxSizeY /  2;
        float bottomY = CurrentAttackData.HitboxSizeY / -2;
        
        hitbox.PolygonInstance.SetPoint(4, leftX, topY);
        
        hitbox.PolygonInstance.SetPoint(0, leftX, topY);
        hitbox.PolygonInstance.SetPoint(1, rightX, topY);
        hitbox.PolygonInstance.SetPoint(2, rightX, bottomY);
        hitbox.PolygonInstance.SetPoint(3, leftX, bottomY);

        hitbox.Holder = Holder;
        hitbox.AttachTo(Parent ?? throw new UnreachableException("idk how this weapon's parent is null but here we are"));

        hitbox.AppliesTo = ~Effects.Team;
        
        var holderHitEffects = new EffectBundle(Effects.Team, Effects.Source);
        // holderHitEffects.AddEffect(new KnockbackEffect(Effects.Team, Effects.Source,
        //     CurrentAttackData.ForwardMovementVelocity, Rotation.FromRadians(RotationZ), KnockbackBehavior.Additive, true));
        holderHitEffects.AddEffect(new HitstopEffect(Effects.Team, Effects.Source, TimeSpan.FromMilliseconds(150)));
        
        hitbox.HolderHitEffects = holderHitEffects;
        
        var targetEffects = new EffectBundle(Effects.Team, Effects.Source);
        targetEffects.AddEffect(new DamageEffect(~Effects.Team, Effects.Source, CurrentAttackData.Damage));
        targetEffects.AddEffect(new KnockbackEffect(~Effects.Team, Effects.Source, CurrentAttackData.KnockbackVelocity,
            Rotation.FromRadians(RotationZ), KnockbackBehavior.Replacement));
        targetEffects.AddEffect(new DamageOverTimeEffect(~Effects.Team, Effects.Source, 1, 2, 5, 1));
        targetEffects.AddEffect(new ApplyShatterEffect(~Effects.Team, Effects.Source));
        targetEffects.AddEffect(new WeaknessDamageEffect(~Effects.Team, Effects.Source, .2f));
        targetEffects.AddEffect(new HitstopEffect(~Effects.Team, Effects.Source, TimeSpan.FromMilliseconds(150)));
        
        hitbox.TargetHitEffects = targetEffects;

        return hitbox;
    }
    
    #endregion
}