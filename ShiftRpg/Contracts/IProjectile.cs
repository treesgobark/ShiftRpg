using System.Collections.Generic;

namespace ShiftRpg.Contracts;

public interface IProjectile
{
    IReadOnlyList<object> GetTargetHitEffects();
    IReadOnlyList<object> GetHolderHitEffects();
}