namespace ProjectLoot.Effects.Base;

public interface IEffectHandler
{
    bool CanHandle(IEffect effect);
    void Handle(IEffect effect);
    
    bool IsActive { get; set; }
}

public interface IEffectHandler<in T> : IEffectHandler where T : IEffect
{
    void Handle(T effect);
}
