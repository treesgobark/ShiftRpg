using FlatRedBall;
using Microsoft.Xna.Framework;
using ProjectLoot.Components;
using ProjectLoot.Contracts;
using ProjectLoot.Effects;
using ProjectLoot.Effects.Handlers;

namespace ProjectLoot.Entities;

public abstract partial class Enemy
{
    private const float WeaknessConversionFactor = 1.5f;
    private const float WeaknessDepletionRate = .2f;
    
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
        Health = new HealthComponent(MaxHealth, HealthBarRuntimeInstance);
        Effects = new EffectsComponent { Team = Team.Enemy };
        Shatter = new ShatterComponent(HealthBarRuntimeInstance);
        Weakness = new WeaknessComponent();
        
        Health.DamageModifiers.Upsert("weakness_damage_bonus", new StatModifier<float>(
            effect => Weakness.CurrentWeaknessAmount > 0 && effect.Source.Contains(SourceTag.Gun),
            effect => 1 + Weakness.CurrentWeaknessAmount * WeaknessConversionFactor,
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
        if (Weakness.CurrentWeaknessAmount > 0)
        {
            Weakness.CurrentWeaknessAmount -= TimeManager.SecondDifference * WeaknessDepletionRate;
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
}