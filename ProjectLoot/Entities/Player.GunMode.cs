using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using FlatRedBall.Input;
using ProjectLoot.Controllers;
using ProjectLoot.Effects;
using ProjectLoot.InputDevices;

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
            Parent.GunInstance.Equip(Parent.GameplayInputDevice.GunInputDevice, Parent.Gun.Weapons.CurrentItem);
        }

        protected override void AfterTimedStateActivity()
        {
            if (Parent.GameplayInputDevice.NextWeapon.WasJustPressed)
            {
                Parent.Gun.Weapons.CycleToNextItem();
                Parent.GunInstance.Equip(Parent.GameplayInputDevice.GunInputDevice, Parent.Gun.Weapons.CurrentItem);
            }
            
            if (Parent.GameplayInputDevice.PreviousWeapon.WasJustPressed)
            {
                Parent.Gun.Weapons.CycleToPreviousItem();
                Parent.GunInstance.Equip(Parent.GameplayInputDevice.GunInputDevice, Parent.Gun.Weapons.CurrentItem);
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
            
            if (Parent.GameplayInputDevice.AimInMeleeRange && Parent.MeleeWeapon.Cache.Count > 0)
            {
                return StateMachine.Get<MeleeWeaponMode>();
            }
        
            return null;
        }

        public override void BeforeDeactivate()
        {
            Parent.GunInstance.Unequip();
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
