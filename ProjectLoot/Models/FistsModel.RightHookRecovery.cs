using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using FlatRedBall.Math.Geometry;
using ProjectLoot.Contracts;
using ProjectLoot.Controllers;
using ProjectLoot.Effects;
using ProjectLoot.Entities;
using ProjectLoot.Factories;

namespace ProjectLoot.Models;

partial class FistsModel
{
    private class RightHookRecovery : ParentedTimedState<FistsModel>
    {
        private static TimeSpan Duration => TimeSpan.FromMilliseconds(180);

        public RightHookRecovery(IReadonlyStateMachine stateMachine, ITimeManager timeManager, FistsModel weaponModel)
            : base(stateMachine, timeManager, weaponModel) { }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate() { }

        public override IState? EvaluateExitConditions()
        {
            if (!Parent.IsEquipped)
            {
                return StateMachine.Get<NotEquipped>();
            }

            if (Parent.MeleeWeaponComponent.MeleeWeaponInputDevice.Attack.WasJustPressed)
            {
                return StateMachine.Get<LeftHook>();
            }
            
            if (TimeInState >= Duration)
            {
                return StateMachine.Get<Idle>();
            }

            return null;
        }

        protected override void AfterTimedStateActivity()
        {
        }

        public override void BeforeDeactivate()
        {
        }

        public override void Uninitialize() { }
    }
}
