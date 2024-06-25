using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using FlatRedBall.Input;
using ProjectLoot.Controllers;
using ProjectLoot.Effects.Handlers;
using ProjectLoot.Handlers;

namespace ProjectLoot.Entities;

public partial class Player
{
    protected class GunMode : ParentedTimedState<Player>
    {
        public GunMode(Player parent, IReadonlyStateMachine stateMachine, ITimeManager timeManager)
            : base(parent, stateMachine, timeManager) { }

        public override void Initialize() { }

        protected override void AfterTimedStateActivate()
        {
            Parent.Gun.Cache.IsActive = true;
        }

        protected override void AfterTimedStateActivity()
        {
            if (Parent.GameplayInputDevice.NextWeapon.WasJustPressed)
            {
                Parent.Gun.Cache.CycleToNextWeapon();
            }
            
            if (Parent.GameplayInputDevice.PreviousWeapon.WasJustPressed)
            {
                Parent.Gun.Cache.CycleToPreviousWeapon();
            }
            
            SetRotation();
        }

        public override IState? EvaluateExitConditions()
        {
            if (Parent.GameplayInputDevice.Dash.WasJustPressed)
            {
                return StateMachine.Get<Dashing>();
            }

            if (Parent.GameplayInputDevice.Guard.IsDown)
            {
                return StateMachine.Get<Guarding>();
            }
            
            if (Parent.GameplayInputDevice.AimInMeleeRange)
            {
                return StateMachine.Get<MeleeMode>();
            }
        
            return null;
        }

        public override void BeforeDeactivate()
        {
            Parent.Gun.Cache.IsActive = false;
        }

        public override void Uninitialize() { }
    
        private void SetRotation()
        {
            float? angle = Parent.GameplayInputDevice.Aim.GetAngle();
            
            if (angle is not null)
            {
                Parent.RotationZ = angle.Value;
            }
        }
    }
}
