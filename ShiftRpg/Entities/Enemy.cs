using System.Collections.Generic;
using FlatRedBall;
using Microsoft.Xna.Framework;
using ShiftRpg.Contracts;
using ShiftRpg.Effects;
using ShiftRpg.Effects.Handlers;

namespace ShiftRpg.Entities;

public abstract partial class Enemy : ITakesShatterDamage, ITakesWeaknessDamage, ITakesKnockback
{
    private int _currentHealth;
    private float _currentShatterDamage;
    private float _currentWeaknessAmount;
    
    private const float WeaknessConversionFactor = 0.04f;
    private const float WeaknessDepletionRate = 10f;

    /// <summary>
    /// Initialization logic which is executed only one time for this Entity (unless the Entity is pooled).
    /// This method is called when the Entity is added to managers. Entities which are instantiated but not
    /// added to managers will not have this method called.
    /// </summary>
    private void CustomInitialize()
    {
        var hudParent = gumAttachmentWrappers[0];
        hudParent.ParentRotationChangesRotation = false;
        Team                                    = Team.Enemy;
        CurrentHealth                           = MaxHealth;
        PersistentEffects                       = new List<IPersistentEffect>();
        RecentEffects = new List<(Guid EffectId, double EffectTime)>();
            
        HealthBarRuntimeInstance.SetAllToZero();
        HealthBarRuntimeInstance.MainBarProgressPercentage = CurrentHealthPercentage;

        HandlerCollection = new EffectHandlerCollection(this);
        HandlerCollection.Add(new DamageHandler(this));
        HandlerCollection.Add(new ShatterDamageHandler(this));
        HandlerCollection.Add(new ApplyShatterDamageHandler(this));
        HandlerCollection.Add(new WeaknessDamageHandler(this));
        HandlerCollection.Add(new KnockbackHandler(this));
        
        DamageModifiers.Upsert("weakness_damage_bonus", new StatModifier<float>(
            effect => CurrentWeaknessAmount > 0 && effect.Source.Contains(SourceTag.Gun),
            effect => 1 + WeaknessProgressPercentage * WeaknessConversionFactor,
            ModifierCategory.Multiplicative));
    }

    private void CustomActivity()
    {
        if (CurrentWeaknessAmount > 0)
        {
            CurrentWeaknessAmount -= TimeManager.SecondDifference * WeaknessDepletionRate;
        }
    }

    private void CustomDestroy()
    {


    }

    private static void CustomLoadStaticContent(string contentManagerName)
    {


    }

    // public void HandlePersistentEffects()
    // {
    //     List<IEffect> effects = [];
    //
    //     for (var i = PersistentEffects.Count - 1; i >= 0; i--)
    //     {
    //         var effect = PersistentEffects[i];
    //         if (effect is DamageOverTimeEffect { ShouldApply: true } dot)
    //         {
    //             effects.Add(dot.GetDamageEffect());
    //             if (dot.RemainingTicks <= 0)
    //             {
    //                 PersistentEffects.RemoveAt(i);
    //             }
    //         }
    //     }
    //
    //     if (effects.Count > 0)
    //     {
    //         HandleEffects(effects);
    //     }
    // }
        
    #region IEffectReceiver

    IReadOnlyEffectHandlerCollection IReadOnlyEffectReceiver.HandlerCollection => HandlerCollection;
    public IEffectHandlerCollection HandlerCollection { get; protected set; }
    public IList<IPersistentEffect> PersistentEffects { get; protected set; }
    public IStatModifierCollection<float> DamageModifiers { get; } = new StatModifierCollection<float>();
    public Team Team { get; protected set; }
    public IList<(Guid EffectId, double EffectTime)> RecentEffects { get; protected set; }
        
    #endregion
        
    public float CurrentHealthPercentage => 100f * CurrentHealth / MaxHealth;

    public virtual float CurrentHealth
    {
        get => _currentHealth;
        set
        {
            _currentHealth = (int)MathHelper.Clamp(value, -1, MaxHealth); 
            HealthBarRuntimeInstance.MainBarProgressPercentage = CurrentHealthPercentage;
            if (CurrentHealth <= 0)
            {
                Destroy();
            }
        }
    }

    public double LastDamageTime { get; set; }

    public float ShatterSubProgressPercentage => CurrentHealth == 0 ? 100f : 100f * CurrentShatterDamage / CurrentHealth;
    public float CurrentShatterDamage
    {
        get => _currentShatterDamage;
        set
        {
            _currentShatterDamage = value;
            _currentShatterDamage = Math.Min(MaxShatterDamageAmount, _currentShatterDamage);
            _currentShatterDamage = Math.Min(CurrentHealth, _currentShatterDamage);
            HealthBarRuntimeInstance.ShatterBarProgressPercentage = ShatterSubProgressPercentage;
        }
    }
        
    public float MaxShatterDamagePercentage => 20;
    public int MaxShatterDamageAmount => (int)(MaxShatterDamagePercentage / 100f * MaxHealth);
        
    public float CurrentWeaknessAmount
    {
        get => _currentWeaknessAmount;
        set
        {
            _currentWeaknessAmount = value;
            _currentWeaknessAmount                                 = Math.Clamp(_currentWeaknessAmount, 0, MaxWeaknessAmount);
            HealthBarRuntimeInstance.WeaknessBarProgressPercentage =  WeaknessProgressPercentage;
        }
    }
        
    public float MaxWeaknessAmount => 100;
    public float WeaknessProgressPercentage => CurrentWeaknessAmount / MaxWeaknessAmount * 100f;
}