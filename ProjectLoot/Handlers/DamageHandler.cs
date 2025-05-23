using ANLG.Utilities.Core.NonStaticUtilities;
using FlatRedBall.Graphics;
using FlatRedBall.Screens;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.Factories;
using ProjectLoot.Handlers.Base;
using ProjectLoot.Screens;

namespace ProjectLoot.Effects.Handlers;

public class DamageHandler : EffectHandler<DamageEffect>, IUpdateable
{
    private IHealthComponent Health { get; }
    private ITransformComponent Transform { get; }
    private ITimeManager TimeManager { get; }
    private IDestroyable? Destroyable { get; }
    private IWeaknessComponent? Weakness { get; }

    public DamageHandler(IEffectsComponent effects, IHealthComponent health, ITransformComponent transform, ITimeManager timeManager,
        IDestroyable? destroyable = null, IWeaknessComponent? weakness = null) : base(effects)
    {
        Health = health;
        Transform = transform;
        TimeManager = timeManager;
        Destroyable = destroyable;
        Weakness = weakness;
    }

    protected override void HandleInternal(DamageEffect effect)
    {
        float finalDamage = effect.Value;
        
        ApplyDamageModifiers(effect, ref finalDamage);
        ApplyDamage(effect, finalDamage);
        CreateDamageNumber(effect, finalDamage);
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
        if (Health.CurrentHealth <= 0)
        {
            Destroyable?.Destroy();
        }
    }
    
    protected virtual void CreateDamageNumber(DamageEffect effect, float finalDamage)
    {
        DamageNumberFactory.CreateNew()
                           .SetStartingValues(finalDamage, 1, Transform.Position, effect.Source, Effects.Team);
    }
}
