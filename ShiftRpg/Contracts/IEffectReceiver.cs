using System;
using System.Collections.Generic;
using ShiftRpg.Effects;

namespace ShiftRpg.Contracts;

public interface IEffectReceiver
{
    IList<(Guid EffectId, double EffectTime)> RecentEffects { get; }
    Team Team { get; set; }
    
    void HandleEffects(IReadOnlyList<IEffect> effects);
}