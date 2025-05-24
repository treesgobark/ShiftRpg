using FlatRedBall;
using Microsoft.Xna.Framework;
using ProjectLoot.Components.Interfaces;

namespace ProjectLoot.Components;

public class SpriteComponent : ISpriteComponent
{
    private Sprite StoredSprite { get; }
    private int _stopCount;
    private float _cachedAnimationSpeed;

    public SpriteComponent(Sprite storedSprite)
    {
        StoredSprite = storedSprite;
    }
    
    private int StopCount
    {
        get => _stopCount;
        set
        {
#if DEBUG
            if (value < 0)
            {
                throw new InvalidOperationException("Tried to resume motion when it wasn't already stopped.");
            }
#endif
            _stopCount = value;
        }
    }

    public float AnimationSpeed
    {
        get => StoredSprite.AnimationSpeed;
        set
        {
            if (!IsStopped)
            {
                StoredSprite.AnimationSpeed = value;
            }
            else
            {
                _cachedAnimationSpeed = value;
            }
        }
    }

    private bool IsStopped => StopCount > 0;

    public Color Color { get => StoredSprite.Color; set => StoredSprite.Color = value; }

    public void StopAnimation()
    {
        if (StopCount == 0)
        {
            _cachedAnimationSpeed = StoredSprite.AnimationSpeed;
            StoredSprite.AnimationSpeed = 0;
        }

        StopCount++;
    }

    public void ResumeAnimation()
    {
        StopCount--;

        if (StopCount == 0)
        {
            StoredSprite.AnimationSpeed = _cachedAnimationSpeed;
        }
    }
}