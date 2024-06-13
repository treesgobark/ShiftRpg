using System.Collections.Generic;
using FlatRedBall;
using ShiftRpg.Contracts;

namespace ShiftRpg.Effects.Handlers;

public class EffectHandlerCollection : IEffectHandlerCollection
{
    protected IEffectReceiver Receiver { get; }
    protected List<Type> HandlerOrder = [];
    protected Dictionary<Type, IEffectHandler> Handlers = new();

    public EffectHandlerCollection(IEffectReceiver receiver)
    {
        Receiver = receiver;
    }

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
        if (Receiver.RecentEffects.Any(t => t.EffectId == bundle.EffectId))
        {
            return;
        }
        
        Receiver.RecentEffects.Add((bundle.EffectId, TimeManager.CurrentScreenTime));
        
        foreach (var key in HandlerOrder)
        {
            var handler = Handlers[key];
            if (bundle.TryGetEffect(key, out object effect))
            {
                handler.Handle(effect);
            }
        }
    }
}