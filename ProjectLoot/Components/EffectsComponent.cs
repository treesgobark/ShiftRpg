using System.Collections.Generic;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.Effects;
using ProjectLoot.Effects.Base;
using ProjectLoot.Handlers.Base;

namespace ProjectLoot.Components;

public class EffectsComponent : IEffectsComponent
{
    private readonly IEffectHandlerCollection _handlerCollection = new ListEffectHandlerCollection();
    
    private IEffectBundle? _currentlyProcessingEffectBundle;
    private readonly Queue<IEffectBundle> _bundleQueue = new();

    public required Team Team { get; set; }
    
    public SourceTag Source { get; set; }

    public void Handle(IEffectBundle bundle)
    {
        _bundleQueue.Enqueue(bundle);

        if (_currentlyProcessingEffectBundle is not null)
        {
            return;
        }

        while (_bundleQueue.Count > 0)
        {
            _currentlyProcessingEffectBundle = _bundleQueue.Dequeue();
            _handlerCollection.Handle(_currentlyProcessingEffectBundle);
            _currentlyProcessingEffectBundle = null;
        }
    }

    public void Handle<T>(T effect) where T : IEffect
    {
        EffectBundle bundle = new();
        bundle.AddEffect(effect);
        
        Handle(bundle);
    }

    public void AddHandler<T>(IEffectHandler<T> handler) where T : IEffect
    {
        _handlerCollection.Add<T>(handler);
    }

    public void Activity()
    {
        _handlerCollection.Activity();
    }
}
