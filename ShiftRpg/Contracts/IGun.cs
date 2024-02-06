using System;
using System.Collections.Generic;
using FlatRedBall.Input;

namespace ShiftRpg.Contracts;

public interface IGun
{
    Action<IReadOnlyList<object>> ApplyImpulse { set; }
    
    void Equip(IGunInputDevice inputDevice);
    void Unequip();
}