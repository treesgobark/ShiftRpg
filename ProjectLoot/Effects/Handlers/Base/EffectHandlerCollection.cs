using System.Collections.Generic;
using FlatRedBall;
using ProjectLoot.Contracts;

namespace ProjectLoot.Effects.Handlers;

public class EffectHandlerCollection : IEffectHandlerCollection
{
    private List<EffectLog> RecentEffects { get; } = [];
    private List<Type> HandlerOrder { get; } = [];
    private Dictionary<Type, IEffectHandler> Handlers { get; } = [];
    private Dictionary<Type, IPersistentEffectHandler> PersistentHandlers { get; } = [];

    public void Add<T>(IEffectHandler<T> handler, int index = -1)
    {
        Type type = typeof(T);
        if (Handlers.ContainsKey(type))
        {
            throw new InvalidOperationException($"Handler already exists for {type.Name}");
        }

        if (index >= 0)
        {
            HandlerOrder.Insert(index, type);
            Handlers[type] = handler;
        }
        else
        {
            HandlerOrder.Add(type);
            Handlers[type] = handler;
        }
        
        if (handler is IPersistentEffectHandler pHandler)
        {
            PersistentHandlers[type] = pHandler;
        }
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

    public void Activity()
    {
        foreach (Type key in HandlerOrder)
        {
            if (PersistentHandlers.TryGetValue(key, out IPersistentEffectHandler? pHandler))
            {
                pHandler.Activity();
            }
        }
    }

    public void Handle(IEffectBundle bundle)
    {
        if (RecentEffects.Any(t => t.EffectId == bundle.EffectId))
        {
            return;
        }
        
        RecentEffects.Add(new EffectLog(bundle.EffectId, TimeManager.CurrentScreenTime));
        
        foreach (Type key in HandlerOrder)
        {
            IEffectHandler handler = Handlers[key];
            if (bundle.TryGetEffect(key, out object effect))
            {
                handler.Handle(effect);
            }
        }
    }
    
    private record EffectLog(Guid EffectId, double EffectTime);
}