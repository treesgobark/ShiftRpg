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
    private class LeftJabRecovery : ParentedTimedState<FistsModel>
    {
        private static TimeSpan Duration => TimeSpan.FromMilliseconds(240);

        public LeftJabRecovery(IReadonlyStateMachine stateMachine, ITimeManager timeManager, FistsModel weaponModel)
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
                return StateMachine.Get<RightHook>();
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
