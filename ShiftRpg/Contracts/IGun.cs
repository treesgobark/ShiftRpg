using System;
using System.Collections.Generic;
using ShiftRpg.Effects;
using ShiftRpg.Entities;

namespace ShiftRpg.Contracts;

public interface IGun : IWeapon<IGunInputDevice>
{
    Projectile SpawnProjectile();
}