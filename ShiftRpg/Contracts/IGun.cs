using System;
using System.Collections.Generic;

namespace ShiftRpg.Contracts;

public interface IGun
{
    Action<IReadOnlyList<object>> ApplyHolderEffects { set; }
    
    void Equip(IGunInputDevice inputDevice);
    void Unequip();
}