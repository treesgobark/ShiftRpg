using ProjectLoot.Contracts;
using ProjectLoot.Effects;
using ProjectLoot.Effects.Base;
using ProjectLoot.Handlers.Base;

namespace ProjectLoot.Components.Interfaces;

public interface IEffectsComponent
{
    Team Team { get; set; }
    SourceTag Source { get; set; }

    void Handle(IEffectBundle bundle);
    void Handle<T>(T effect) where T : IEffect;
    void AddHandler<T>(IEffectHandler<T> handler) where T : IEffect;
}
