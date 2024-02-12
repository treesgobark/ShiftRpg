using System;
using ANLG.Utilities.FlatRedBall.Constants;
using Microsoft.Xna.Framework;
using ShiftRpg.Contracts;
using ShiftRpg.Effects;

namespace ShiftRpg.Controllers.Gun;

public class Ready(IGun obj) : GunController(obj)
{
    protected GunController? NextState { get; set; }

    public override void Initialize() { }

    public override void CustomActivity()
    {
        if (Parent.InputDevice.Fire.WasJustPressed || Parent is { FiringType: FiringType.Automatic, InputDevice.Fire.IsDown: true })
        {
            FireBullet();
            NextState = Get<Recovery>();
        }

        if (Parent.InputDevice.Reload.WasJustPressed)
        {
            NextState = Get<Reloading>();
        }
    }

    public override GunController? EvaluateExitConditions()
    {
        if (Parent.MagazineRemaining <= 0)
        {
            return Get<Reloading>();
        }
        
        if (NextState is not null)
        {
            return NextState;
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