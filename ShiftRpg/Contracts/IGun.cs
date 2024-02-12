using System;
using System.Collections.Generic;
using ANLG.Utilities.FlatRedBall.Controllers;
using ShiftRpg.Controllers.Gun;
using ShiftRpg.DataTypes;
using ShiftRpg.Effects;
using ShiftRpg.Entities;

namespace ShiftRpg.Contracts;

public interface IGun : IWeapon<IGunInputDevice>, IHasControllers<IGun, GunController>
{
    int MagazineRemaining { get; }
    int MagazineSize { get; }
    TimeSpan TimePerRound { get; }
    TimeSpan ReloadTime { get; }
    
    void Fire();
    void StartReload();
    void FillMagazine();
}