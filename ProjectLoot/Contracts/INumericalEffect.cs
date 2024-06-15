using System.Collections.Generic;
using System.Numerics;

namespace ProjectLoot.Contracts;

public interface INumericalEffect<T> : IEffect where T : INumber<T>
{
    T Value { get; }
    
    List<T> AdditiveIncreases { get; }
    List<T> MultiplicativeIncreases { get; }
}