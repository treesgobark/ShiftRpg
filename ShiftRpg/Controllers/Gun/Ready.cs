using System;
using ANLG.Utilities.FlatRedBall.Constants;
using Microsoft.Xna.Framework;
using ShiftRpg.Contracts;
using ShiftRpg.Effects;

namespace ShiftRpg.Controllers.Gun;

public class Ready(IGun obj) : GunController(obj)
{
    protected GunController? NextState { get; set; }
    protected bool IsFiring { get; set; }

    public override void Initialize() { }

    public override void CustomActivity()
    {
        if (Parent.InputDevice.Fire.WasJustPressed)
        {
            FireBullet();
        }

        if (Parent.InputDevice.Reload.WasJustPressed)
        {
            NextState = Get<Reloading>();
        }
    }

    public override GunController? EvaluateExitConditions()
    {
        if (NextState is not null)
        {
            return NextState;
        }
        
        if (Parent.MagazineRemaining <= 0)
        {
            return Get<Reloading>();
        }

        return null;
    }

    public override void BeforeDeactivate()
    {
        NextState = null;
    }

    private void FireBullet()
    {
        Parent.Fire();
    }
}