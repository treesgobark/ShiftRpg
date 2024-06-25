namespace ProjectLoot.Contracts;

public interface IEffectHandler
{
    void Handle(object effect);
    
    bool IsActive { get; set; }
}
