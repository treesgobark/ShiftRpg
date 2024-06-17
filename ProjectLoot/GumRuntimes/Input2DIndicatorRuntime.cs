using System;
using System.Collections.Generic;
using System.Linq;
using FlatRedBall.Input;

namespace ProjectLoot.GumRuntimes
{
    public partial class Input2DIndicatorRuntime
    {
        partial void CustomInitialize () 
        {
        }

        public void SetPosition(I2DInput input, float maxRadius)
        {
            StickSprite.X = input.X * maxRadius;
            StickSprite.Y = -input.Y * maxRadius;
        }
    }
}
