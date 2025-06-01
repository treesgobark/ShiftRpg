using ProjectLoot.Contracts;
using ProjectLoot.Effects.Base;

namespace ProjectLoot.Handlers.Base;

public interface IReadOnlyEffectHandlerCollection
{
    void Handle(IEffectBundle bundle);
}

public interface IEffectHandlerCollection : IReadOnlyEffectHandlerCollection
{
    void Add<T>(IEffectHandler     handler) where T: IEffect;
    void Add<T>(IEffectHandler     handler, int index) where T: IEffect;
    void Replace<T>(IEffectHandler handler) where T: IEffect;
    void Remove<T>(IEffectHandler  handler) where T : IEffect;

    void Activity();
}