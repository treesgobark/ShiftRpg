using System.Threading.Tasks;
using FlatRedBall;
using ProjectLoot.Components.Interfaces;

namespace ProjectLoot.Components;

public class DamageableSpriteComponent : SpriteComponent, IDamageableSpriteComponent
{
    private readonly Sprite _storedSprite;
    private string? _previousChainName;

    private bool _isPlaying;

    public DamageableSpriteComponent(Sprite storedSprite) : base(storedSprite)
    {
        _storedSprite = storedSprite;
    }
    
    public TimeSpan RemainingAnimationTime { get; set; }

    public void PlayDamageAnimation()
    {
        if (!_isPlaying)
        {
            _previousChainName = _storedSprite.CurrentChainName;
            _isPlaying         = true;
        }
        
        _storedSprite.CurrentChainName = "Damage";
        RemainingAnimationTime = TimeSpan.FromSeconds(_storedSprite.CurrentChain.TotalLength);
    }

    public void RestorePreviousAnimation()
    {
        _storedSprite.CurrentChainName = _previousChainName;
        _isPlaying = false;
    }
}