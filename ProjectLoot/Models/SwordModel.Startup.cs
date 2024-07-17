// using ANLG.Utilities.Core.NonStaticUtilities;
// using ANLG.Utilities.Core.States;
//
// namespace ProjectLoot.Models;
//
// public partial class SwordModel
// {
//     protected class Startup : TimedState
//     {
//         private SwordModel SwordModel { get; }
//         
//         public Startup(IReadonlyStateMachine stateMachine, ITimeManager timeManager, SwordModel swordModel) : base(stateMachine, timeManager)
//         {
//             SwordModel = swordModel;
//         }
//         
//         public override void Initialize() { }
//
//         protected override void AfterTimedStateActivate()
//         {
//         }
//
//         protected override void AfterTimedStateActivity()
//         {
//         }
//
//         public override IState? EvaluateExitConditions()
//         {
//             if (TimeInState > SwordModel.CurrentAttackData.StartupTimeSpan)
//             {
//                 return StateMachine.Get<Active>();
//             }
//
//             return null;
//         }
//
//         public override void BeforeDeactivate() { }
//
//         public override void Uninitialize() { }
//     }
// }