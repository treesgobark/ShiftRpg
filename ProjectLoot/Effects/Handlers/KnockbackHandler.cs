using FlatRedBall.Math;
using ProjectLoot.Components.Interfaces;

namespace ProjectLoot.Effects.Handlers;

public class KnockbackHandler : EffectHandler<KnockbackEffect>
{
    private IPositionable Position { get; }
    private IEffectsComponent Effects { get; }

    public KnockbackHandler(IPositionable position, IEffectsComponent effects)
    {
        Position = position;
        Effects = effects;
    }
    
    public override void Handle(KnockbackEffect effect)
    {
        bool valid = ValidateEffect(effect);
        if (!valid) { return; }
        
        Position.XVelocity = effect.KnockbackVector.X;
        Position.YVelocity = effect.KnockbackVector.Y;
    }
    
    protected virtual bool ValidateEffect(KnockbackEffect effect)
    {
        if (!Effects.Team.IsSubsetOf(effect.AppliesTo)) { return false; }

        return true;
    }
}