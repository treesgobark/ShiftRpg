using ANLG.Utilities.Core.NonStaticUtilities;
using FlatRedBall;
using FlatRedBall.Math;
using Microsoft.Xna.Framework;
using ProjectLoot.Contracts;
using ProjectLoot.Effects;

namespace ProjectLoot.Components.Interfaces;

public interface IMeleeWeaponComponent
{
    PositionedObject Holder { get; }
    Team Team { get; }
    IMeleeWeaponInputDevice MeleeWeaponInputDevice { get; }
}