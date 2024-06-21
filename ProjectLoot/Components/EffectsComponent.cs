using System.Collections.Generic;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.Effects;
using ProjectLoot.Effects.Handlers;

namespace ProjectLoot.Components;

public class EffectsComponent : IEffectsComponent
{
    private Team? _team;
    public IEffectHandlerCollection HandlerCollection { get; } = new EffectHandlerCollection();

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
}
