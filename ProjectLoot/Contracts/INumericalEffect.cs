using System.Collections.Generic;
using System.Numerics;
using ProjectLoot.Effects;

namespace ProjectLoot.Contracts;

public interface INumericalEffect<T> where T : INumber<T>
{
    T Value { get; }
    SourceTag Source { get; }
    
    List<T> AdditiveIncreases { get; }
    List<T> MultiplicativeIncreases { get; }
}