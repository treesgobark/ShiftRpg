using ANLG.Utilities.FlatRedBall.Extensions;
using FlatRedBall;
using FlatRedBall.Math;
using Microsoft.Xna.Framework;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Factories;

namespace ProjectLoot.Effects.Handlers;

public class DamageHandler : EffectHandler<DamageEffect>
{
    private IHealthComponent Health { get; }
    private IPositionable Position { get; }
    private IWeaknessComponent? Weakness { get; }

    public DamageHandler(IEffectsComponent effects, IHealthComponent health, IPositionable position,
        IWeaknessComponent? weakness = null) : base(effects)
    {
        Health = health;
        Position = position;
        Weakness = weakness;
    }
    
    public override void Handle(DamageEffect effect)
    {
        bool valid = ValidateEffect(effect);
        if (!valid) { return; }
        
        float finalDamage = effect.Value;
        
        ApplyDamageModifiers(effect, ref finalDamage);
        ApplyDamage(effect, finalDamage);
        CreateDamageNumber(effect, finalDamage);
    }
    
    protected virtual bool ValidateEffect(DamageEffect effect)
    {
        if (!effect.AppliesTo.Contains(Effects.Team)) { return false; }

        return true;
    }
    
    protected virtual void ApplyDamageModifiers(DamageEffect damageEffect, ref float finalDamage)
    {
        Health.DamageModifiers.ModifyEffect(damageEffect);
        
        finalDamage = (int)((damageEffect.AdditiveIncreases.Sum() + 1) * finalDamage);
        
        if (damageEffect.MultiplicativeIncreases.Count > 0)
        {
            finalDamage = (int)(damageEffect.MultiplicativeIncreases.Aggregate((f1, f2) => f1 * f2) * finalDamage);
        }
    }
    
    protected virtual void ApplyDamage(DamageEffect effect, float finalDamage)
    {
        Health.CurrentHealth -= finalDamage;
        Health.LastDamageTime = TimeManager.CurrentScreenTime;

        if (Weakness is not null && effect.Source.Contains(SourceTag.Gun))
        {
            Weakness.CurrentWeaknessPercentage -= 2 * 100f * finalDamage / Health.MaxHealth;
        }
    }
    
    protected virtual void CreateDamageNumber(DamageEffect effect, float finalDamage)
    {
        DamageNumberFactory.CreateNew()
            .SetStartingValues(finalDamage.ToString(), 1, Position.PositionAsVec3());
    }
}
