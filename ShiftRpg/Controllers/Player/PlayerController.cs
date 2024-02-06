using System;
using ANLG.Utilities.FlatRedBall.Controllers;
using FlatRedBall;

namespace ShiftRpg.Controllers.Player;

public abstract class PlayerController(Entities.Player obj) : EntityController<Entities.Player, PlayerController>(obj)
{
    private double StartTime { get; set; } = -69420;
    protected double TimeInState
    {
        get
        {
            if (StartTime < 0)
            {
                throw new ArgumentException("you must run the base OnActivate to use TimeInState");
            }
            
            return TimeManager.CurrentScreenSecondsSince(StartTime);
        }
    }

    public override void OnActivate()
    {
        StartTime = TimeManager.CurrentScreenTime;
    }
}