using Microsoft.Xna.Framework;

namespace ProjectLoot.Components.Interfaces;

public interface ITransformComponent
{
    public Vector3 Position { get; set; }
    public Vector3 Velocity { get; set; }
    public Vector3 Acceleration { get; set; }

    void StopMotion();
    void ResumeMotion();
}