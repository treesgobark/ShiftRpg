using ProjectLoot.Components.Interfaces;
using ProjectLoot.Effects;
using ProjectLoot.Factories;
using ProjectLoot.Handlers.Base;

namespace ProjectLoot.Handlers;

public class CorpseSpawnHandler : EffectHandler<DeathEffect>
{
    private readonly ITransformComponent _transformComponent;
    private readonly ICorpseInformationComponent _corpseInformationComponent;

    public CorpseSpawnHandler(IEffectsComponent           effects, ITransformComponent transformComponent,
                              ICorpseInformationComponent corpseInformationComponent) : base(effects)
    {
        _transformComponent         = transformComponent;
        _corpseInformationComponent = corpseInformationComponent;
    }

    public override void Handle(DeathEffect effect)
    {
        var corpse = CorpseFactory.CreateNew(_transformComponent.Position);
        corpse.InitializeFromEntity(_corpseInformationComponent, _transformComponent);
    }   
}
