// using ANLG.Utilities.Core.NonStaticUtilities;
// using ANLG.Utilities.Core.States;
// using ProjectLoot.Controllers;
//
// namespace ProjectLoot.Components;
//
// partial class MeleeWeaponComponent
// {
//     protected class Recovery : ParentedTimedState<MeleeWeaponComponent>
//     {
//         public Recovery(MeleeWeaponComponent parent, IReadonlyStateMachine stateMachine, ITimeManager timeManager) : base(parent, stateMachine, timeManager) { }
//
//         public override void Initialize() { }
//
//         protected override void AfterTimedStateActivate() { }
//
//         protected override void AfterTimedStateActivity() { }
//
//         public override IState? EvaluateExitConditions()
//         {
//             if (TimeInState > Parent.CurrentAttackData.RecoveryTimeSpan)
//             {
//                 return StateMachine.Get<Idle>();
//             }
//
//             return null;
//         }
//
//         public override void BeforeDeactivate()
//         {
//             Parent.Holder.SetInputEnabled(true);
//         }
//
//         public override void Uninitialize() { }
//     }
// }