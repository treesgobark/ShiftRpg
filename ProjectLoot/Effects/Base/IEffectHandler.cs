namespace ProjectLoot.Effects.Base;

public interface IEffectHandler
{
    void Handle(IEffect effect);
    
    bool IsActive { get; set; }
}
