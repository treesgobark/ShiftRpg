using ProjectLoot.Contracts;

namespace ProjectLoot.Effects.Handlers;

public interface IReadOnlyEffectHandlerCollection
{
    void Handle(IEffectBundle bundle);
}

public interface IEffectHandlerCollection : IReadOnlyEffectHandlerCollection
{
    void Add<T>(IEffectHandler<T> handler);
    void Replace<T>(IEffectHandler<T> handler);
    void Remove<T>(IEffectHandler<T> handler);
}