using System.Collections.Generic;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.Effects;
using ProjectLoot.Effects.Handlers;

namespace ProjectLoot.Components;

public class EffectsComponent : IEffectsComponent
{
    public IEffectHandlerCollection HandlerCollection { get; } = new EffectHandlerCollection();
    public required Team Team { get; set; }
    public void Handle(IEffectBundle bundle)
    {
        HandlerCollection.Handle(bundle);
    }
}
