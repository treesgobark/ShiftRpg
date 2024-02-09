using System.Collections.Generic;

namespace ShiftRpg.Contracts;

public interface IEffectReceiver
{
    void HandleEffects(IEnumerable<object> effects);
}