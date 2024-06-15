namespace ProjectLoot.Contracts;

public interface IEffectHandler
{
    void Handle(object effect);
}

public interface IEffectHandler<in T> : IEffectHandler
{
    void Handle(T effect);
}
