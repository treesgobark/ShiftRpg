using ProjectLoot.Contracts;
using ProjectLoot.Effects;
using ProjectLoot.Effects.Handlers;

namespace ProjectLoot.Components.Interfaces;

public interface IEffectsComponent
{
    IEffectHandlerCollection HandlerCollection { get; }
    Team Team { get; set; }
    SourceTag Source { get; set; }
    bool IsInvulnerable { get; set; }

    void Handle(IEffectBundle bundle);
}
