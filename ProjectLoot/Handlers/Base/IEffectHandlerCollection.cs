using ProjectLoot.Contracts;

namespace ProjectLoot.Effects.Handlers;

public interface IReadOnlyEffectHandlerCollection
{
    void Handle(IEffectBundle bundle);
}

public interface IEffectHandlerCollection : IReadOnlyEffectHandlerCollection
{
    void Add<T>(T handler) where T: class;
    void Add<T>(T handler, int index) where T: class;
    void Replace<T>(T handler) where T: class;
    void Remove<T>(T handler) where T: class;

    void Activity();
    void Activity<T>() where T: class, IUpdateable;
}