using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.Effects;
using ProjectLoot.Effects.Base;
using ProjectLoot.Handlers.Base;

namespace ProjectLoot.Components;

public class EffectsComponent : IEffectsComponent
{
    private Team? _team;
    private readonly IEffectHandlerCollection _handlerCollection = new ListEffectHandlerCollection();

    public Team Team
    {
        get => _team ?? throw new InvalidOperationException("Team not set");
        set => _team = value;
    }
    
    public SourceTag Source { get; set; }

    public void Handle(IEffectBundle bundle)
    {
        _handlerCollection.Handle(bundle);
    }

    public void Handle<T>(T effect) where T : IEffect
    {
        EffectBundle bundle = new();
        bundle.AddEffect(effect);
        
        _handlerCollection.Handle(bundle);
    }

    public void AddHandler<T>(IEffectHandler handler) where T : class
    {
        _handlerCollection.Add<T>(handler);
    }

    public void Activity()
    {
        _handlerCollection.Activity();
    }
}
