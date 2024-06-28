using ProjectLoot.Contracts;

namespace ProjectLoot.Effects.Handlers;

public interface IReadOnlyEffectHandlerCollection
{
    void Handle(IEffectBundle bundle);
}

public interface IEffectHandlerCollection : IReadOnlyEffectHandlerCollection
{
    void Add<T>(IEffectHandler     handler) where T: class;
    void Add<T>(IEffectHandler     handler, int index) where T: class;
    void Replace<T>(IEffectHandler handler) where T: class;
    void Remove<T>(IEffectHandler  handler) where T : class;

    void Activity();
}