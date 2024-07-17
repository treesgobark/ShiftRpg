using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.Effects;
using ProjectLoot.Effects.Handlers;

namespace ProjectLoot.Components;

public class EffectsComponent : IEffectsComponent
{
    private Team? _team;
    public IEffectHandlerCollection HandlerCollection { get; set; } = new EffectHandlerCollection();

    public Team Team
    {
        get => _team ?? throw new InvalidOperationException("Team not set");
        set => _team = value;
    }
    
    public SourceTag Source { get; set; }

    public void Handle(IEffectBundle bundle)
    {
        HandlerCollection.Handle(bundle);
    }

    public void Handle<T>(T effect)
    {
        EffectBundle bundle = new();
        bundle.AddEffect(effect);
        
        HandlerCollection.Handle(bundle);
    }

    public void Activity()
    {
        HandlerCollection.Activity();
    }
}
