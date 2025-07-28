// using ANLG.Utilities.NonStaticUtilities;
// using ANLG.Utilities.States;
//
// namespace ProjectLoot.Models;
//
// public partial class SwordModel
// {
//     protected class Startup : TimedState
//     {
//         private SwordModel SwordModel { get; }
//         
//         public Startup(IReadonlyStateMachine states, ITimeManager timeManager, SwordModel swordModel) : base(states, timeManager)
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
//                 return _states.Get<Active>();
//             }
//
//             return null;
//         }
//
//         public override void BeforeDeactivate(IState? nextState) { }
//
//         public override void Uninitialize() { }
//     }
// }