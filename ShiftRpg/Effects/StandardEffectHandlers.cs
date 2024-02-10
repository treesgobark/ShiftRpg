using System;
using System.Linq;
using FlatRedBall;
using ShiftRpg.Contracts;

namespace ShiftRpg.Effects;

public static class StandardEffectHandlers
{
    public static IEffect HandleStandardDamage<T>(this IEffect effect, T receiver) where T : ITakesDamage
    {
        if (effect is not DamageEffect damage) { return effect; }
        if (!receiver.Team.IsSubsetOf(damage.AppliesTo)) { return effect; }
        if (receiver.TimeSinceLastDamage < receiver.InvulnerabilityTimeAfterDamage) { return effect; }

        int finalDamage = damage.Damage;
        
        finalDamage = (int)((damage.AdditiveIncreases.Sum() + 1)                          * finalDamage);

        if (damage.MultiplicativeIncreases.Count > 0)
        {
            finalDamage = (int)(damage.MultiplicativeIncreases.Aggregate((f1, f2) => f1 * f2) * finalDamage);
        }
        
        receiver.TakeDamage(finalDamage);
        receiver.LastDamageTime = TimeManager.CurrentScreenTime;
        
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
}