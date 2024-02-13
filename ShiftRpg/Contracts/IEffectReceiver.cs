using System;
using System.Collections.Generic;
using ShiftRpg.Effects;

namespace ShiftRpg.Contracts;

public interface IEffectReceiver
{
    IList<(Guid EffectId, double EffectTime)> RecentEffects { get; }
    IList<IPersistentEffect> PersistentEffects { get; }
    Team Team { get; }
    
    void HandleEffects(IReadOnlyList<IEffect> effects);
}