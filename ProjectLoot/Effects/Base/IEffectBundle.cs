using System.Collections;
using System.Collections.Generic;

namespace ProjectLoot.Effects.Base;

public interface IEffectBundle : IEnumerable<IEffect>
{
    Guid EffectId { get; }
    bool IgnoreUniqueness { get; }

    bool TryGetEffect(Type type, out IEffect effect);
}

public class EffectBundle : IEffectBundle
{
    public static readonly EffectBundle Empty = new();

    public Guid EffectId { get; } = Guid.NewGuid();
    public bool IgnoreUniqueness { get; }

    private readonly Dictionary<Type, IEffect> _effects = new();

    public EffectBundle(bool ignoreUniqueness = false)
    {
        IgnoreUniqueness = ignoreUniqueness;
    }

    public void AddEffect<T>(T effect) where T : IEffect
    {
        if (_effects.ContainsKey(typeof(T)))
        {
            throw new InvalidOperationException($"Handler already exists for {typeof(T).Name}");
        }

        _effects[typeof(T)] = effect;
    }

    public void UpsertEffect<T>(T effect) where T : IEffect
    {
        _effects[typeof(T)] = effect;
    }

    public bool TryGetEffect(Type type, out IEffect effect)
    {
        if (_effects.TryGetValue(type, out effect!))
        {
            return true;
        }

        effect = null!;
        return false;
    }

    public IEnumerator<IEffect> GetEnumerator() => _effects.Values.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _effects.Values.GetEnumerator();
}