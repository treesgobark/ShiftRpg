using System.Collections;
using System.Collections.Generic;
using ShiftRpg.Effects;

namespace ShiftRpg.Contracts;

public interface IEffectBundle : IEnumerable<object>
{
    Team AppliesTo { get; }
    SourceTag Source { get; }
    Guid EffectId { get; }

    bool TryGetEffect(Type type, out object effect);
}

public class EffectBundle : IEffectBundle
{
    public static readonly EffectBundle Empty = new((Team)(-1), (SourceTag)(-1), Guid.Empty);
    
    public EffectBundle(Team appliesTo, SourceTag source, Guid effectId)
    {
        AppliesTo = appliesTo;
        Source    = source;
        EffectId  = effectId;
    }
    
    public EffectBundle(Team appliesTo, SourceTag source)
    {
        AppliesTo = appliesTo;
        Source    = source;
        EffectId  = Guid.NewGuid();
    }

    public Team AppliesTo { get; init; }
    public SourceTag Source { get; init; }
    public Guid EffectId { get; init; }
    
    protected readonly Dictionary<Type, object> Effects = new();
    
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