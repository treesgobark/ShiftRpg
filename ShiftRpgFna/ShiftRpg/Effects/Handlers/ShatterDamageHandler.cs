using ShiftRpg.Contracts;

namespace ShiftRpg.Effects.Handlers;

public class ShatterDamageHandler : EffectHandler<ShatterDamageEffect>
{
    private ITakesShatterDamage Receiver { get; }
    
    public ShatterDamageHandler(ITakesShatterDamage receiver)
    {
        Receiver = receiver;
    }
    
    public override void Handle(ShatterDamageEffect effect)
    {
        bool valid = ValidateEffect(effect);
        if (!valid) { return; }
        
        float finalDamage = effect.ShatterDamage;
        
        ApplyDamageModifiers(effect, ref finalDamage);
        ApplyDamage(effect, finalDamage);
    }
    
    protected virtual bool ValidateEffect(ShatterDamageEffect effect)
    {
        if (!Receiver.Team.IsSubsetOf(effect.AppliesTo)) { return false; }
        if (Receiver.IsInvulnerable) { return false; }

        return true;
    }
    
    protected virtual void ApplyDamageModifiers(ShatterDamageEffect damageEffect, ref float finalDamage)
    {
        finalDamage = (int)((damageEffect.AdditiveIncreases.Sum() + 1) * finalDamage);
        
        if (damageEffect.MultiplicativeIncreases.Count > 0)
        {
            finalDamage = (int)(damageEffect.MultiplicativeIncreases.Aggregate((f1, f2) => f1 * f2) * finalDamage);
        }
    }
    
    protected virtual void ApplyDamage(ShatterDamageEffect effect, float finalDamage)
    {
        Receiver.CurrentShatterDamage  += finalDamage;
        Receiver.CurrentShatterDamage = Math.Min(Receiver.CurrentShatterDamage, Receiver.MaxShatterDamagePercentage * Receiver.MaxHealth);
    }
}