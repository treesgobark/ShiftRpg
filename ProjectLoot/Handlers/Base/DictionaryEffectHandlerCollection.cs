using System.Collections.Generic;
using FlatRedBall;
using ProjectLoot.Contracts;
using ProjectLoot.Effects.Base;

namespace ProjectLoot.Handlers.Base;

public class DictionaryEffectHandlerCollection : IEffectHandlerCollection
{
    private List<EffectLog> RecentEffects { get; } = [];
    private List<Type> HandlerOrder { get; } = [];
    private Dictionary<Type, IEffectHandler> Handlers { get; } = [];
    private Dictionary<Type, IUpdateable> Updateables { get; } = [];

    public void Add<T>(IEffectHandler handler) where T : class => Add<T>(handler, HandlerOrder.Count);

    public void Add<T>(IEffectHandler handler, int index) where T: class
    {
        Type type = typeof(T);
        
        if (Handlers.ContainsKey(type))
        {
            throw new InvalidOperationException($"Handler already exists for {type.Name}");
        }

        HandlerOrder.Insert(index, type);

        UpsertHandlerToDictionaries(handler, type);
    }

    public void Replace<T>(IEffectHandler handler) where T: class
    {
        Type type = typeof(T);
        if (!Handlers.ContainsKey(type))
        {
            throw new InvalidOperationException($"Handler does not exist for {type.Name}");
        }

        int index = HandlerOrder.IndexOf(type);
        HandlerOrder[index] = type;

        UpsertHandlerToDictionaries(handler, type);
    }

    public void Remove<T>(IEffectHandler handler) where T: class
    {
        Type type = typeof(T);
        if (!Handlers.ContainsKey(type))
        {
            throw new InvalidOperationException($"Handler does not exist for {type.Name}");
        }

        HandlerOrder.Remove(type);
        Handlers.Remove(type);
        Updateables.Remove(type);
    }

    public void Activity()
    {
        foreach (Type key in HandlerOrder)
        {
            if (Updateables.TryGetValue(key, out IUpdateable? updateable))
            {
                updateable.Activity();
            }
        }
    }

    public void Activity<T>() where T : class, IUpdateable
    {
        Type type = typeof(T);
        
        if (Updateables.TryGetValue(type, out IUpdateable? updateable))
        {
            updateable.Activity();
        }
        else
        {
            throw new InvalidOperationException(
                $"Cannot update handler of type {type.Name} because one is not registered.");
        }
    }

    public void Handle(IEffectBundle bundle)
    {
        if (!bundle.IgnoreUniqueness && RecentEffects.Any(t => t.EffectId == bundle.EffectId))
        {
            return;
        }
        
        RecentEffects.Add(new EffectLog(bundle.EffectId, TimeManager.CurrentScreenTime));
        
        foreach (Type key in HandlerOrder)
        {
            IEffectHandler handler = Handlers[key];
            if (bundle.TryGetEffect(key, out IEffect effect))
            {
                handler.Handle(effect);
            }
        }
    }

    private void UpsertHandlerToDictionaries<T>(T handler, Type type) where T : class
    {
        if (handler is IEffectHandler eHandler)
        {
            Handlers[type] = eHandler;
        }

        if (handler is IUpdateable updateable)
        {
            Updateables[type] = updateable;
        }
    }
    
    private record EffectLog(Guid EffectId, double EffectTime);
}