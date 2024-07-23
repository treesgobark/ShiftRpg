using System.Collections.Generic;
using System.Numerics;
using ProjectLoot.Effects.Base;

namespace ProjectLoot.Contracts;

public interface IStatModifierCollection<T> where T : INumber<T>
{
    // IEnumerable<StatModifier<T>> Get(SourceTag damageSource);
    List<StatModifier<T>> GetActiveModifiers(INumericalEffect<T> effect);
    
    void Upsert(string id, StatModifier<T> modifier);

    bool Delete(string id);

    void ModifyEffect(INumericalEffect<T> effect);
}

public record StatModifier<T>(
    Func<INumericalEffect<T>, bool> Predicate,
    Func<INumericalEffect<T>, T> Factory,
    ModifierCategory Category,
    MatchingStrategy MatchingStrategy = MatchingStrategy.Subset
) where T : INumber<T>;

public enum ModifierCategory
{
    Additive,
    Multiplicative,
}

public enum MatchingStrategy
{
    /// <summary>
    /// Modifer will apply if the incoming source has at least the tags specified in the modifer
    /// </summary>
    Subset,
    
    /// <summary>
    /// Modifier will apply if the incoming source has each tag and only the tags specified in the modifer
    /// </summary>
    Exact,
    
    /// <summary>
    /// Modifer will apply if the incoming source has at least one of the tags specified in the modifer
    /// </summary>
    Any,
}
