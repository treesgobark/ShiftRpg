using FlatRedBall.Math;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Models;

namespace ProjectLoot.Contracts;

public interface IWeaponHolder : IPositionable
{
    IEffectsComponent Effects { get; }
    bool InputEnabled { get; set; }

    IEffectBundle ModifyTargetEffects(IEffectBundle effects);
}
