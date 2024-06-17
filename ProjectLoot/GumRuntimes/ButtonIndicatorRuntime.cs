using System;
using System.Collections.Generic;
using System.Linq;
using FlatRedBall.Input;

namespace ProjectLoot.GumRuntimes
{
    public partial class ButtonIndicatorRuntime
    {
        partial void CustomInitialize () 
        {
        }

        public void SetPressedState(IPressableInput input)
        {
            CurrentIsPressedState = input.IsDown ? IsPressed.Pressed : IsPressed.NotPressed;
        }
    }
}
