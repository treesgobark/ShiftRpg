using System;
using System.Linq;
using FlatRedBall;
using Microsoft.Xna.Framework;
using ShiftRpg.Contracts;
using ShiftRpg.Factories;

namespace ShiftRpg.Effects;

public static class StandardEffectHandlers
{
    public static IEffect HandleStandardDamage<T>(this IEffect effect, T receiver) where T : PositionedObject, ITakesDamage
    {
        if (effect is not DamageEffect damage) { return effect; }
        if (!receiver.Team.IsSubsetOf(damage.AppliesTo)) { return effect; }
        if (receiver.TimeSinceLastDamage < receiver.InvulnerabilityTimeAfterDamage) { return effect; }

        float finalDamage = damage.Damage;

        finalDamage = (int)((damage.AdditiveIncreases.Sum() + 1) * finalDamage);

        if (damage.MultiplicativeIncreases.Count > 0)
        {
            finalDamage = (int)(damage.MultiplicativeIncreases.Aggregate((f1, f2) => f1 * f2) * finalDamage);
        }
        
        receiver.TakeDamage(finalDamage);
        receiver.LastDamageTime = TimeManager.CurrentScreenTime;
        receiver.RecentEffects.Add((effect.EffectId, TimeManager.CurrentScreenTime));

        DamageNumberFactory.CreateNew()
            .SetStartingValues(finalDamage.ToString(), float.Sqrt(MathHelper.Max(finalDamage, 1)), receiver.Position);
        
        return effect;
    }
    
    public static IEffect HandleStandardShatterDamage<T>(this IEffect effect, T receiver) where T : PositionedObject, ITakesShatterDamage
    {
        if (effect is not ShatterDamageEffect damage) { return effect; }
        if (!receiver.Team.IsSubsetOf(damage.AppliesTo)) { return effect; }

        int finalDamage = damage.ShatterDamage;

        finalDamage = (int)((damage.AdditiveIncreases.Sum() + 1) * finalDamage);

        if (damage.MultiplicativeIncreases.Count > 0)
        {
            finalDamage = (int)(damage.MultiplicativeIncreases.Aggregate((f1, f2) => f1 * f2) * finalDamage);
        }
        
        receiver.TakeShatterDamage(finalDamage);
        receiver.RecentEffects.Add((effect.EffectId, TimeManager.CurrentScreenTime));

        return effect;
    }

    public static IEffect HandleStandardApplyShatter<T>(this IEffect effect, T receiver)
        where T : PositionedObject, ITakesShatterDamage
    {
        if (effect is not ApplyShatterEffect damage) { return effect; }
        if (!receiver.Team.IsSubsetOf(damage.AppliesTo)) { return effect; }

        if (receiver.CurrentShatterDamage > 0)
        {
            receiver.HandleEffects([new DamageEffect(effect.AppliesTo, SourceTag.Shatter, receiver.CurrentShatterDamage)]);
            receiver.ResetShatterDamage();
        }

        receiver.RecentEffects.Add((effect.EffectId, TimeManager.CurrentScreenTime));
        return effect;
    }
    
    public static IEffect HandleStandardKnockback<T>(this IEffect effect, T receiver) where T : PositionedObject, IEffectReceiver
    {
        if (effect is not KnockbackEffect knockback) { return effect; }
        if (!receiver.Team.IsSubsetOf(knockback.AppliesTo)) { return effect; }

        receiver.Velocity += knockback.KnockbackVector;
        
        receiver.RecentEffects.Add((effect.EffectId, TimeManager.CurrentScreenTime));
        return effect;
    }

    public static IEffect HandleStandardPersistentEffect<T>(this IEffect effect, T receiver)
        where T : IEffectReceiver
    {
        if (effect is not IPersistentEffect persistentEffect) { return effect; }
        if (!receiver.Team.IsSubsetOf(persistentEffect.AppliesTo)) { return effect; }

        receiver.PersistentEffects.Add(persistentEffect);
        receiver.RecentEffects.Add((effect.EffectId, TimeManager.CurrentScreenTime));
        
        return effect;
    }
}