using FlatRedBall;
using Microsoft.Xna.Framework;
using ProjectLoot.Components;
using ProjectLoot.Contracts;
using ProjectLoot.Effects;
using ProjectLoot.Effects.Handlers;

namespace ProjectLoot.Entities;

public abstract partial class Enemy
{
    private int _currentHealth;
    private float _currentShatterDamage;
    private float _currentWeaknessAmount;
    
    private const float WeaknessConversionFactor = 0.04f;
    private const float WeaknessDepletionRate = 10f;
    
    public HealthComponent Health { get; private set; }
    public EffectsComponent Effects { get; private set; }
    public ShatterComponent Shatter { get; private set; }
    public WeaknessComponent Weakness { get; private set; }

    /// <summary>
    /// Initialization logic which is executed only one time for this Entity (unless the Entity is pooled).
    /// This method is called when the Entity is added to managers. Entities which are instantiated but not
    /// added to managers will not have this method called.
    /// </summary>
    private void CustomInitialize()
    {
        Health = new HealthComponent { MaxHealth = MaxHealth };
        Effects = new EffectsComponent { Team = Team.Enemy };
        Shatter = new ShatterComponent();
        Weakness = new WeaknessComponent();
        
        Health.DamageModifiers.Upsert("weakness_damage_bonus", new StatModifier<float>(
            effect => CurrentWeaknessAmount > 0 && effect.Source.Contains(SourceTag.Gun),
            effect => 1 + WeaknessProgressPercentage * WeaknessConversionFactor,
            ModifierCategory.Multiplicative));

        Effects.HandlerCollection.Add(new DamageHandler(Health, Effects, this));
        Effects.HandlerCollection.Add(new ShatterDamageHandler(Effects, Health, Shatter));
        Effects.HandlerCollection.Add(new ApplyShatterDamageHandler(Effects, Shatter, Health));
        Effects.HandlerCollection.Add(new WeaknessDamageHandler(Effects, Health, Weakness));
        Effects.HandlerCollection.Add(new KnockbackHandler(this, Effects));
        
        var hudParent = gumAttachmentWrappers[0];
        hudParent.ParentRotationChangesRotation = false;
            
        HealthBarRuntimeInstance.Reset();
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