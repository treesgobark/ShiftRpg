using ANLG.Utilities.Core;
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
    Vector3 HolderGameplayCenterPosition { get; }
    Rotation AttackDirection { get; }
    IMeleeWeaponModel CurrentMeleeWeapon { get; }

    void AttachObjectToAttackOrigin(PositionedObject obj);
}