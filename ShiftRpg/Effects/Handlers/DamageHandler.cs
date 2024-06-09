using System.Linq;
using ANLG.Utilities.FlatRedBall.Extensions;
using FlatRedBall;
using Microsoft.Xna.Framework;
using ShiftRpg.Contracts;
using ShiftRpg.Factories;

namespace ShiftRpg.Effects.Handlers;

public class DamageHandler : IEffectHandler<DamageEffect>
{
    protected ITakesDamage Receiver { get; }
    
    public DamageHandler(ITakesDamage receiver)
    {
        Receiver = receiver;
    }
    
    public void Handle(object effect)
    {
        if (effect is DamageEffect damageEffect)
        {
            Handle(damageEffect);
        }
        else
        {
            throw new ArgumentException("Invalid effect type", nameof(effect));
        }
    }
    
    public void Handle(DamageEffect effect)
    {
        bool valid = ValidateEffect(effect);
        if (!valid) { return; }
        
        float finalDamage = effect.Damage;
        
        ApplyDamageModifiers(effect, ref finalDamage);
        ApplyDamage(effect, finalDamage);
        CreateDamageNumber(effect, finalDamage);
    }
    
    protected virtual bool ValidateEffect(DamageEffect effect)
    {
        if (!Receiver.Team.IsSubsetOf(effect.AppliesTo)) { return false; }
        if (Receiver.IsInvulnerable) { return false; }

        return true;
    }
    
    protected virtual void ApplyDamageModifiers(DamageEffect damageEffect, ref float finalDamage)
    {
        finalDamage = (int)((damageEffect.AdditiveIncreases.Sum() + 1) * finalDamage);
        
        if (damageEffect.MultiplicativeIncreases.Count > 0)
        {
            finalDamage = (int)(damageEffect.MultiplicativeIncreases.Aggregate((f1, f2) => f1 * f2) * finalDamage);
        }
    }
    
    protected virtual void ApplyDamage(DamageEffect effect, float finalDamage)
    {
        Receiver.CurrentHealth  -= finalDamage;
        Receiver.LastDamageTime =  TimeManager.CurrentScreenTime;
    }
    
    protected virtual void CreateDamageNumber(DamageEffect effect, float finalDamage)
    {
        DamageNumberFactory.CreateNew()
            .SetStartingValues(finalDamage.ToString(), float.Sqrt(MathHelper.Max(finalDamage, 1)), Receiver.GetPosition());
    }
}