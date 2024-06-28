using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using ANLG.Utilities.FlatRedBall.Extensions;
using ProjectLoot.Controllers;
using ProjectLoot.DataTypes;

namespace ProjectLoot.Entities;

public partial class Player
{
    protected class Dashing : ParentedTimedState<Player>
    {
        private TopDownValues CachedValues { get; set; } = new();

        public Dashing(Player parent, IReadonlyStateMachine stateMachine, ITimeManager timeManager)
            : base(parent, stateMachine, timeManager)
        {
        }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate()
        {
            CachedValues.IsUsingCustomDeceleration = Parent.CurrentMovement.IsUsingCustomDeceleration;
            CachedValues.DecelerationTime = Parent.CurrentMovement.DecelerationTime;
            
            Parent.CurrentMovement.IsUsingCustomDeceleration = false;
            Parent.CurrentMovement.DecelerationTime = 100;
            
            Parent.Velocity = 250f * Parent.GameplayInputDevice.Movement.GetNormalizedPositionOrZero().ToVec3();
        }

        protected override void AfterTimedStateActivity() { }

        public override IState? EvaluateExitConditions()
        {
            if (TimeInState < TimeSpan.FromSeconds(0.15f)) { return null; }

            return (Parent.MeleeWeapon.Cache.Count, Parent.Gun.Weapons.Count,
                    Parent.GameplayInputDevice.AimInMeleeRange) switch
            {
                (> 0, 0, _)       => StateMachine.Get<MeleeWeaponMode>(),
                (0, > 0, _)       => StateMachine.Get<GunMode>(),
                (> 0, > 0, true)  => StateMachine.Get<MeleeWeaponMode>(),
                (> 0, > 0, false) => StateMachine.Get<GunMode>(),
                _                 => StateMachine.Get<Unarmed>()
            };
        }

        public override void BeforeDeactivate()
        {
            Parent.CurrentMovement.IsUsingCustomDeceleration = CachedValues.IsUsingCustomDeceleration;
            Parent.CurrentMovement.DecelerationTime = CachedValues.DecelerationTime;
        }

        public override void Uninitialize() { }
    }
}