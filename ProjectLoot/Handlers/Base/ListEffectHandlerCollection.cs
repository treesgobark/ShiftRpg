using System.Collections.Generic;
using FlatRedBall;
using ProjectLoot.Contracts;
using ProjectLoot.Effects.Base;

namespace ProjectLoot.Handlers.Base;

public class ListEffectHandlerCollection : IEffectHandlerCollection
{
    private List<EffectLog> RecentEffects { get; } = [];
    private List<IEffectHandler> Handlers { get; } = [];
    private List<IUpdateable> Updateables { get; } = [];

    public void Add<T>(IEffectHandler handler) where T : IEffect => Add<T>(handler, Handlers.Count);

    public void Add<T>(IEffectHandler handler, int index) where T: IEffect
    {
        Handlers.Add(handler);

        if (handler is IUpdateable updateable)
        {
            Updateables.Add(updateable);
        }
    }

    public void Replace<T>(IEffectHandler handler) where T: IEffect
    {
        throw new NotImplementedException();
    }

    public void Remove<T>(IEffectHandler handler) where T: IEffect
    {
        Handlers.Remove(handler);

        if (handler is IUpdateable updateable)
        {
            Updateables.Remove(updateable);
        }
    }

    public void Activity()
    {
        foreach (IUpdateable updateable in Updateables)
        {
            updateable.Activity();
        }
    }

    public void Handle(IEffectBundle bundle)
    {
        if (!bundle.IgnoreUniqueness && RecentEffects.Any(t => t.EffectId == bundle.EffectId))
        {
            return;
        }
        
        RecentEffects.Add(new EffectLog(bundle.EffectId, TimeManager.CurrentScreenTime));
        
        foreach (IEffectHandler handler in Handlers)
        {
            foreach (IEffect effect in bundle)
            {
                if (handler.CanHandle(effect))
                {
                    handler.Handle(effect);
                }
            }
        }
    }
    
    private record EffectLog(Guid EffectId, double EffectTime);
}