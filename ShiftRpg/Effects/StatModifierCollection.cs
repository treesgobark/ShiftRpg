using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ShiftRpg.Contracts;

namespace ShiftRpg.Effects;

public class StatModifierCollection<T> : IStatModifierCollection<T> where T : INumber<T>
{
    private readonly Dictionary<string, StatModifier<T>> _damageModifiers = new();
    
    public List<StatModifier<T>> GetActiveModifiers(INumericalEffect<T> effect)
    {
        List<StatModifier<T>> modifiers = [];
        foreach ((string id, var damageModifier) in _damageModifiers)
        {
            bool match = damageModifier.Predicate(effect);
            if (match)
            {
                modifiers.Add(damageModifier);
            }
        }

        return modifiers;
    }

    /// <summary>
    /// Add or update a damage modifier based on its id
    /// </summary>
    public void Upsert(string id, StatModifier<T> modifier)
    {
        _damageModifiers[id] = modifier;
    }

    public void ModifyEffect(INumericalEffect<T> effect)
    {
        var modifiers = GetActiveModifiers(effect);
        foreach (var modifier in modifiers)
        {
            switch (modifier.Category)
            {
                case ModifierCategory.Additive:
                    effect.AdditiveIncreases.Add(modifier.Factory(effect));
                    break;
                case ModifierCategory.Multiplicative:
                    effect.MultiplicativeIncreases.Add(modifier.Factory(effect));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(modifier.Category), modifier.Category, "Category out of range");
            }
        }
    }
}
