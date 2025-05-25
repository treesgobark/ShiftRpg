using System.Threading.Tasks;

namespace ProjectLoot.Components.Interfaces;

public interface IDamageableSpriteComponent : ISpriteComponent
{
    TimeSpan RemainingAnimationTime { get; }
    
    void PlayDamageAnimation();
    void RestorePreviousAnimation();
}