using ANLG.Utilities.Core;
using ANLG.Utilities.FlatRedBall.Extensions;
using ANLG.Utilities.States;
using Microsoft.Xna.Framework;
using ProjectLoot.Controllers;
using ProjectLoot.DataTypes;

namespace ProjectLoot.Entities;

public partial class Player
{
    protected class Dashing : ParentedTimedState<Player>
    {
        private readonly IReadonlyStateMachine _states;
        private TopDownValues CachedValues { get; set; } = new();

        public Dashing(Player parent, IReadonlyStateMachine states, ITimeManager timeManager)
            : base(timeManager, parent)
        {
            _states = states;
        }

        protected override void AfterTimedStateActivate()
        {
            CachedValues.IsUsingCustomDeceleration = Parent.CurrentMovement.IsUsingCustomDeceleration;
            CachedValues.DecelerationTime = Parent.CurrentMovement.DecelerationTime;
            
            Parent.CurrentMovement.IsUsingCustomDeceleration = false;
            Parent.CurrentMovement.DecelerationTime = 100;
            
            Parent.Velocity = 400f * Parent.GameplayInputDevice.Movement.GetNormalizedPositionOrZero().ToVec3();
        }

        protected override void AfterTimedStateActivity() { }

        public override IState? EvaluateExitConditions()
        {
            if (TimeInState < TimeSpan.FromSeconds(0.15f)) { return null; }

            return (Parent.MeleeWeaponComponent.IsEmpty, Parent.GunComponent.IsEmpty,
                    Parent.GameplayInputDevice.AimInMeleeRange) switch
            {
                (false, true, _)      => _states.Get<MeleeWeaponMode>(),
                (true, false, _)      => _states.Get<GunMode>(),
                (false, false, true)  => _states.Get<MeleeWeaponMode>(),
                (false, false, false) => _states.Get<GunMode>(),
                _                     => _states.Get<Unarmed>()
            };
        }

        public override void BeforeDeactivate()
        {
            Parent.CurrentMovement.IsUsingCustomDeceleration = CachedValues.IsUsingCustomDeceleration;
            Parent.CurrentMovement.DecelerationTime = CachedValues.DecelerationTime;

            if (Parent.Velocity.Length() > Parent.CurrentMovement.MaxSpeed)
            {
                Parent.Velocity = Parent.Velocity.AtLength(Parent.CurrentMovement.MaxSpeed);
            }
        }
    }
}