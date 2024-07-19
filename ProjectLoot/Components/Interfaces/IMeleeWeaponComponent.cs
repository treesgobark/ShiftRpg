using ANLG.Utilities.Core.NonStaticUtilities;
using FlatRedBall;
using Microsoft.Xna.Framework;
using ProjectLoot.Contracts;
using ProjectLoot.Effects;

namespace ProjectLoot.Components.Interfaces;

public interface IMeleeWeaponComponent
{
    Team Team { get; }
    IMeleeWeaponInputDevice MeleeWeaponInputDevice { get; }
    Vector3 HolderSpritePosition { get; }
    Rotation AttackDirection { get; }

    void AttachObjectToAttackOrigin(PositionedObject obj);
}