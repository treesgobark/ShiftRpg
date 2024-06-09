using ShiftRpg.Contracts;

namespace ShiftRpg.Effects.Handlers;

public class ApplyShatterDamageHandler : IEffectHandler<ApplyShatterEffect>
{
    protected ITakesShatterDamage Receiver { get; }
    
    public ApplyShatterDamageHandler(ITakesShatterDamage receiver)
    {
        Receiver = receiver;
    }

    public void Handle(object effect)
    {
        if (effect is ApplyShatterEffect castedEffect)
        {
            Handle(castedEffect);
        }
        else
        {
            throw new ArgumentException("Invalid effect type", nameof(effect));
        }
    }
    
    public void Handle(ApplyShatterEffect effect)
    {
        bool valid = ValidateEffect(effect);
        if (!valid) { return; }
        
         if (Receiver.CurrentShatterDamage > 0)
         {
             var bundle = new EffectBundle(effect.AppliesTo, SourceTag.Shatter);
             bundle.AddEffect(new DamageEffect(effect.AppliesTo, SourceTag.Shatter, Receiver.CurrentShatterDamage));
             Receiver.HandlerCollection.Handle(bundle);
             Receiver.CurrentShatterDamage = 0;
         }
    }
    
    protected virtual bool ValidateEffect(ApplyShatterEffect effect)
    {
        if (!Receiver.Team.IsSubsetOf(effect.AppliesTo)) { return false; }
        if (Receiver.IsInvulnerable) { return false; }

        return true;
    }
}