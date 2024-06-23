using FlatRedBall.Math;
using ProjectLoot.Components.Interfaces;

namespace ProjectLoot.Contracts;

public interface IWeaponHolder : IPositionable
{
    IEffectsComponent Effects { get; }
    bool InputEnabled { get; }
    void SetInputEnabled(bool isEnabled);

    IEffectBundle ModifyTargetEffects(IEffectBundle effects);
}
