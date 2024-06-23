using ANLG.Utilities.FlatRedBall.Extensions;
using ANLG.Utilities.FlatRedBall.States;
using Microsoft.Xna.Framework;
using ProjectLoot.DataTypes;

namespace ProjectLoot.Entities;

public partial class Player
{
    protected class Dashing : TimedState<Player>
    {
        private TopDownValues CachedValues { get; set; } = new();
        
        public Dashing(Player parent, IReadonlyStateMachine stateMachine) : base(parent, stateMachine)
        {
        }

        public override void Initialize() { }

        protected override void AfterTimedStateActivate()
        {
            CachedValues.IsUsingCustomDeceleration = Parent.CurrentMovement.IsUsingCustomDeceleration;
            CachedValues.DecelerationTime = Parent.CurrentMovement.DecelerationTime;
            
            Parent.CurrentMovement.IsUsingCustomDeceleration = false;
            Parent.CurrentMovement.DecelerationTime = 100;
            
            Parent.Effects.IsInvulnerable = true;
            Parent.Velocity = 250f * Parent.GameplayInputDevice.Movement.GetNormalizedPositionOrZero().ToVec3();
        }
        
        protected override void AfterTimedStateActivity() { }

        public override IState? EvaluateExitConditions()
        {
            if (TimeInState < 0.15f) { return null; }
            
            return Parent.GameplayInputDevice.AimInMeleeRange
                ? StateMachine.Get<MeleeMode>()
                : StateMachine.Get<GunMode>();
        }

        public override void BeforeDeactivate()
        {
            Parent.CurrentMovement.IsUsingCustomDeceleration = CachedValues.IsUsingCustomDeceleration;
            Parent.CurrentMovement.DecelerationTime = CachedValues.DecelerationTime;
            
            Parent.Effects.IsInvulnerable = false;
        }

        public override void Uninitialize() { }
    }
}