using ProjectLoot.Contracts;
using ProjectLoot.Effects;
using ProjectLoot.Effects.Base;
using ProjectLoot.Effects.Handlers;
using ProjectLoot.Handlers.Base;

namespace ProjectLoot.Components.Interfaces;

public interface IEffectsComponent
{
    IEffectHandlerCollection HandlerCollection { get; set; }
    Team Team { get; set; }
    SourceTag Source { get; set; }

    void Handle(IEffectBundle bundle);
    void Handle<T>(T effect) where T : IEffect;
}
