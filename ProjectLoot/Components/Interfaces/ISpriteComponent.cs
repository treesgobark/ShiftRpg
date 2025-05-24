using Microsoft.Xna.Framework;

namespace ProjectLoot.Components.Interfaces;

public interface ISpriteComponent
{
    Color Color { get; set; }
    
    void StopAnimation();
    void ResumeAnimation();
}