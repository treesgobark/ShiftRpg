using FlatRedBall.Math;
using ProjectLoot.Components.Interfaces;

namespace ProjectLoot.Effects.Handlers;

public class KnockbackHandler : EffectHandler<KnockbackEffect>
{
    private IPositionable Position { get; }
    private IHitstopComponent? Hitstop { get; }

    public KnockbackHandler(IEffectsComponent effects, IPositionable position, IHitstopComponent? hitstop = null) : base(effects)
    {
        Position = position;
        Hitstop = hitstop;
    }
    
    public override void Handle(KnockbackEffect effect)
    {
        bool valid = ValidateEffect(effect);
        if (!valid) { return; }

        if (Hitstop is { IsStopped: true })
        {
            Hitstop.StoredVelocity = effect.KnockbackVector;
        }
        else
        {
            if (effect is { KnockbackBehavior: KnockbackBehavior.Replacement })
            {
                Position.XVelocity = effect.KnockbackVector.X;
                Position.YVelocity = effect.KnockbackVector.Y;
            }
            else if (effect is { KnockbackBehavior: KnockbackBehavior.Additive })
            {
                Position.XVelocity += effect.KnockbackVector.X;
                Position.YVelocity += effect.KnockbackVector.Y;
            }
        }
    }
    
    protected virtual bool ValidateEffect(KnockbackEffect effect)
    {
        if (!Effects.Team.IsSubsetOf(effect.AppliesTo)) { return false; }

        return true;
    }
}