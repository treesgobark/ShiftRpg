using ProjectLoot.Components.Interfaces;
using ProjectLoot.Effects;
using ProjectLoot.Entities;
using ProjectLoot.Factories;
using ProjectLoot.Handlers.Base;

namespace ProjectLoot.Handlers;

public class CorpseSpawnHandler : EffectHandler<DeathEffect>
{
    private readonly ITransformComponent _transformComponent;
    private readonly ICorpseInformationComponent _corpseInformationComponent;
    private readonly IHitstopComponent _hitstopComponent;

    public CorpseSpawnHandler(IEffectsComponent           effects, ITransformComponent transformComponent,
                              ICorpseInformationComponent corpseInformationComponent, IHitstopComponent hitstopComponent) : base(effects)
    {
        _transformComponent         = transformComponent;
        _corpseInformationComponent = corpseInformationComponent;
        _hitstopComponent      = hitstopComponent;
    }

    public override void Handle(DeathEffect effect)
    {
        if (_hitstopComponent.IsStopped)
        {
            _corpseInformationComponent.HitstopDuration = _hitstopComponent.RemainingHitstopTime;
        }
        
        CorpseFactory.CreateNew(_transformComponent.Position)
                     .InitializeFromEntity(_corpseInformationComponent, _transformComponent);
    }   
}
