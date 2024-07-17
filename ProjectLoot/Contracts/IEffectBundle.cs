using System.Collections;
using System.Collections.Generic;
using ProjectLoot.Effects;

namespace ProjectLoot.Contracts;

public interface IEffectBundle : IEnumerable<object>
{
    Guid EffectId { get; }
    bool IgnoreUniqueness { get; }

    bool TryGetEffect(Type type, out object effect);
}

public class EffectBundle : IEffectBundle
{
    public static readonly EffectBundle Empty = new();

    public Guid EffectId { get; } = Guid.NewGuid();
    public bool IgnoreUniqueness { get; }

    private readonly Dictionary<Type, object> Effects = new();

    public EffectBundle(bool ignoreUniqueness = false)
    {
        IgnoreUniqueness = ignoreUniqueness;
    }

    public void AddEffect<T>(T effect)
    {
        if (Effects.ContainsKey(typeof(T)))
        {
            throw new InvalidOperationException($"Handler already exists for {typeof(T).Name}");
        }

        Effects[typeof(T)] = effect;
    }

    public bool TryGetEffect(Type type, out object effect)
    {
        if (Effects.TryGetValue(type, out effect!))
        {
            return true;
        }

        effect = null!;
        return false;
    }

    public IEnumerator<object> GetEnumerator() => Effects.Values.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => Effects.Values.GetEnumerator();
}