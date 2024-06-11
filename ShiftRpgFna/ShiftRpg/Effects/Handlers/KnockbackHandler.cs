using FlatRedBall;

namespace ShiftRpg.Effects.Handlers;

public class KnockbackHandler : EffectHandler<KnockbackEffect>
{
    private ITakesKnockback Receiver { get; }
    
    public KnockbackHandler(ITakesKnockback receiver)
    {
        Receiver = receiver;
    }
    
    public override void Handle(KnockbackEffect effect)
    {
        bool valid = ValidateEffect(effect);
        if (!valid) { return; }
        
        Receiver.XVelocity += effect.KnockbackVector.X;
        Receiver.YVelocity += effect.KnockbackVector.Y;
    }
    
    protected virtual bool ValidateEffect(KnockbackEffect effect)
    {
        if (!Receiver.Team.IsSubsetOf(effect.AppliesTo)) { return false; }

        return true;
    }
}