using Microsoft.Xna.Framework;

namespace ProjectLoot.Components.Interfaces;

public interface IHitstopComponent
{
    bool IsStopped { get; set; }
    Vector3 StoredVelocity { get; set; }
    Vector3 StoredAcceleration { get; set; }
    float StoredAnimationSpeed { get; set; }
    double HitstopEndTime { get; set; }

    void Stop();
    void Resume();
}