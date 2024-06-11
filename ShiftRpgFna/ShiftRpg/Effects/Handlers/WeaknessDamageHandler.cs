using System.Linq;
using FlatRedBall;
using ShiftRpg.Contracts;

namespace ShiftRpg.Effects.Handlers;

public class WeaknessDamageHandler : EffectHandler<WeaknessDamageEffect>
{
    private ITakesWeaknessDamage Receiver { get; }

    public WeaknessDamageHandler(ITakesWeaknessDamage receiver)
    {
        Receiver = receiver;
    }
    
    public override void Handle(WeaknessDamageEffect effect)
    {
        bool valid = ValidateEffect(effect);
        if (!valid) { return; }
        
        float finalDamage = effect.WeaknessDamage;
        
        ApplyDamageModifiers(effect, ref finalDamage);
        ApplyDamage(effect, finalDamage);
    }
    
    protected virtual bool ValidateEffect(WeaknessDamageEffect effect)
    {
        if (!Receiver.Team.IsSubsetOf(effect.AppliesTo)) { return false; }
        if (Receiver.IsInvulnerable) { return false; }

        return true;
    }
    
    protected virtual void ApplyDamageModifiers(WeaknessDamageEffect damageEffect, ref float finalDamage)
    {
        finalDamage = (int)((damageEffect.AdditiveIncreases.Sum() + 1) * finalDamage);
        
        if (damageEffect.MultiplicativeIncreases.Count > 0)
        {
            finalDamage = (int)(damageEffect.MultiplicativeIncreases.Aggregate((f1, f2) => f1 * f2) * finalDamage);
        }
    }
    
    protected virtual void ApplyDamage(WeaknessDamageEffect effect, float finalDamage)
    {
        Receiver.CurrentWeaknessAmount  += finalDamage;
    }
}