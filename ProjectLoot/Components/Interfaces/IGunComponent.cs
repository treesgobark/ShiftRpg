using ANLG.Utilities.Core;
using Microsoft.Xna.Framework;
using ProjectLoot.Contracts;
using ProjectLoot.Effects;

namespace ProjectLoot.Components.Interfaces;

public interface IGunComponent
{
    Vector3 BulletOrigin { get; }
    Rotation GunRotation { get; }
    Team Team { get; }
    IGunInputDevice GunInputDevice { get; }
}