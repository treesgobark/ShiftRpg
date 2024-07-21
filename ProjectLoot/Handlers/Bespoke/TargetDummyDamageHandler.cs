using ANLG.Utilities.Core.NonStaticUtilities;
using FlatRedBall.Graphics;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.Effects;
using ProjectLoot.Effects.Handlers;
using ProjectLoot.Factories;

namespace ProjectLoot.Handlers.Bespoke;

public class TargetDummyDamageHandler : EffectHandler<DamageEffect>, IUpdateable
{
    private IHealthComponent Health { get; }
    private ITransformComponent Transform { get; }
    private ITimeManager TimeManager { get; }
    private IDestroyable? Destroyable { get; }
    private IWeaknessComponent? Weakness { get; }

    public TargetDummyDamageHandler(IEffectsComponent effects, IHealthComponent health, ITransformComponent transform, ITimeManager timeManager,
                                    IDestroyable? destroyable = null, IWeaknessComponent? weakness = null) : base(effects)
    {
        Health = health;
        Transform = transform;
        TimeManager = timeManager;
        Destroyable = destroyable;
        Weakness = weakness;
    }

    protected override void Handle(DamageEffect effect)
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

    private void ApplyDamageModifiers(DamageEffect damageEffect, ref float finalDamage)
    {
        Health.DamageModifiers.ModifyEffect(damageEffect);
        
        finalDamage = (int)((damageEffect.AdditiveIncreases.Sum() + 1) * finalDamage);
        
        if (damageEffect.MultiplicativeIncreases.Count > 0)
        {
            finalDamage = (int)(damageEffect.MultiplicativeIncreases.Aggregate((f1, f2) => f1 * f2) * finalDamage);
        }
    }

    private void ApplyDamage(DamageEffect effect, float finalDamage)
    {
        Health.CurrentHealth -= finalDamage;
        Health.LastDamageTime = TimeManager.TotalGameTime;

        if (Weakness is not null && effect.Source.Contains(SourceTag.Gun))
        {
            Weakness.CurrentWeaknessPercentage -= 2 * 100f * finalDamage / Health.MaxHealth;
        }
    }

    public void Activity()
    {
    }
    
    protected virtual void CreateDamageNumber(DamageEffect effect, float finalDamage)
    {
        DamageNumberFactory.CreateNew()
                           .SetStartingValues(finalDamage.ToString(), 1, Transform.Position, effect.Source, Effects.Team);
    }
}
