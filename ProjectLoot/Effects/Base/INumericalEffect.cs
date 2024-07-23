using System.Collections.Generic;
using System.Numerics;

namespace ProjectLoot.Effects.Base;

public interface INumericalEffect<T> where T : INumber<T>
{
    T Value { get; }
    SourceTag Source { get; }
    
    List<T> AdditiveIncreases { get; }
    List<T> MultiplicativeIncreases { get; }
}