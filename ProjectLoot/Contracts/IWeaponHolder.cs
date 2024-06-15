using ProjectLoot.Components.Interfaces;

namespace ProjectLoot.Contracts;

public interface IWeaponHolder
{
    IEffectBundle ModifyTargetEffects(IEffectBundle effects);
    void SetInputEnabled(bool isEnabled);
    IEffectsComponent EffectsComponent { get; }
}