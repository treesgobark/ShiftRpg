namespace ShiftRpg.Contracts;

public interface IWeaponHolder : IReadOnlyEffectReceiver
{
    IEffectBundle ModifyTargetEffects(IEffectBundle effects);
    void SetInputEnabled(bool isEnabled);
}