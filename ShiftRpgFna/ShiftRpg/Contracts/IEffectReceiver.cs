using System.Collections.Generic;
using ShiftRpg.Effects;
using ShiftRpg.Effects.Handlers;

namespace ShiftRpg.Contracts;

public interface IReadOnlyEffectReceiver
{
    IReadOnlyEffectHandlerCollection HandlerCollection { get; }
    Team Team { get; }
}

public interface IEffectReceiver : IReadOnlyEffectReceiver
{
    new IEffectHandlerCollection HandlerCollection { get; }
    IList<(Guid EffectId, double EffectTime)> RecentEffects { get; }
    // IList<IPersistentEffect> PersistentEffects { get; }
}