using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using FlatRedBall.Input;
using Microsoft.Xna.Framework;
using ProjectLoot.Controllers;
using ProjectLoot.Effects;
using ProjectLoot.InputDevices;

namespace ProjectLoot.Entities;

public partial class Player
{
    protected class GunMode : ParentedTimedState<Player>
    {
        public GunMode(Player parent, IReadonlyStateMachine stateMachine, ITimeManager timeManager)
            : base(stateMachine, timeManager, parent) { }

        public override void Initialize() { }

        protected override void AfterTimedStateActivate()
        {
            Parent.GunComponent.Equip();
            Parent.TargetLineSprite.Visible = true;
            Parent.ReticleSprite.Visible = true;
        }

        protected override void AfterTimedStateActivity()
        {
            if (Parent.GameplayInputDevice.NextWeapon.WasJustPressed)
            {
                Parent.GunComponent.CycleToNextWeapon();
            }
            
            if (Parent.GameplayInputDevice.PreviousWeapon.WasJustPressed)
            {
                Parent.GunComponent.CycleToPreviousWeapon();
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
                return Parent.MeleeWeaponComponent.IsEmpty
                    ? StateMachine.Get<Unarmed>()
                    : StateMachine.Get<MeleeWeaponMode>();
            }
        
            return null;
        }

        public override void BeforeDeactivate()
        {
            Parent.GunComponent.Unequip();
            Parent.TargetLineSprite.Visible = false;
            Parent.ReticleSprite.Visible    = false;
        }

        public override void Uninitialize() { }
    
        private void SetRotation()
        {
            float? angle = Parent.GameplayInputDevice.Aim.GetAngle();
            
            if (angle is not null)
            {
                Parent.RotationZ = angle.Value;
                Parent.GameplayCenter.ForceUpdateDependenciesDeep();
            }
        }
    }
}
