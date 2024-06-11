using FlatRedBall;
using ShiftRpg.Contracts;

namespace ShiftRpg.Effects;

public class DamageOverTimeEffect(
    Team appliesTo,
    SourceTag source,
    float damagePerTick,
    double ticksPerSecond,
    int numberOfTicks,
    double activationDelay = -1)
    : IPersistentEffect
{
    public double DamagePerSecond => DamagePerTick * TicksPerSecond;
    public float RemainingDamage => DamagePerTick  * RemainingTicks;
    public float DamagePerTick { get; } = damagePerTick;
    public int RemainingTicks { get; protected set; } = numberOfTicks;
    public double TicksPerSecond { get; } = ticksPerSecond;
    public double SecondsPerTick => 1 / TicksPerSecond;
    public Team AppliesTo { get; } = appliesTo;
    public SourceTag Source { get; } = source;
    public Guid EffectId { get; } = Guid.NewGuid();
    public double LastAppliedTime { get; set; } = TimeManager.CurrentScreenTime;
    public double TimeSinceLastApplication => TimeManager.CurrentScreenSecondsSince(LastAppliedTime);

    public bool ShouldApply => TimeSinceLastApplication > SecondsPerTick;

    public DamageEffect GetDamageEffect()
    {
        var effect = new DamageEffect(AppliesTo, Source, DamagePerTick);
        LastAppliedTime = TimeManager.CurrentScreenTime;
        RemainingTicks--;
        return effect;
    }
}