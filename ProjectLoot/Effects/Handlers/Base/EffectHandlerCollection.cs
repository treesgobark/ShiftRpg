using System.Collections.Generic;
using FlatRedBall;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;

namespace ProjectLoot.Effects.Handlers;

public class EffectHandlerCollection : IEffectHandlerCollection
{
    private List<EffectLog> RecentEffects { get; } = [];
    private List<Type> HandlerOrder { get; } = [];
    private Dictionary<Type, IEffectHandler> Handlers { get; } = new();

    public void Add<T>(IEffectHandler<T> handler)
    {
        Type type = typeof(T);
        if (Handlers.ContainsKey(type))
        {
            throw new InvalidOperationException($"Handler already exists for {type.Name}");
        }

        HandlerOrder.Add(type);
        Handlers[type] = handler;
    }

    public void Replace<T>(IEffectHandler<T> handler)
    {
        Type type = typeof(T);
        if (!Handlers.ContainsKey(type))
        {
            throw new InvalidOperationException($"Handler does not exist for {type.Name}");
        }

        int index = HandlerOrder.IndexOf(type);
        HandlerOrder[index] = type;
        Handlers[type] = handler;
    }

    public void Remove<T>(IEffectHandler<T> handler)
    {
        Type type = typeof(T);
        if (!Handlers.ContainsKey(type))
        {
            throw new InvalidOperationException($"Handler does not exist for {type.Name}");
        }

        HandlerOrder.Remove(type);
        Handlers.Remove(type);
    }

    public void Handle(IEffectBundle bundle)
    {
        if (RecentEffects.Any(t => t.EffectId == bundle.EffectId))
        {
            return;
        }
        
        RecentEffects.Add(new EffectLog(bundle.EffectId, TimeManager.CurrentScreenTime));
        
        foreach (var key in HandlerOrder)
        {
            var handler = Handlers[key];
            if (bundle.TryGetEffect(key, out object effect))
            {
                handler.Handle(effect);
            }
        }
    }
    
    private record EffectLog(Guid EffectId, double EffectTime);
}