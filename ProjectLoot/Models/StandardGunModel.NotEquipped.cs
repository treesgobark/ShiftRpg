using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;

namespace ProjectLoot.Models;

partial class StandardGunModel
{
    private class NotEquipped : TimedState
    {
        private StandardGunModel GunModel { get; }

        public NotEquipped(IReadonlyStateMachine states, ITimeManager timeManager, StandardGunModel gunModel)
            : base(states, timeManager)
        {
            GunModel = gunModel;
        }
    
        public override void Initialize() { }

        protected override void AfterTimedStateActivate(IState? previousState) { }

        public override IState? EvaluateExitConditions()
        {
            if (GunModel.IsEquipped)
            {
                return States.Get<Ready>();
            }
        
            return null;
        }

        protected override void AfterTimedStateActivity() { }

        public override void BeforeDeactivate(IState? nextState)
        {
            GunModel.GunViewModel.MaximumMagazineCount = GunModel.GunData.MagazineSize;
            GunModel.GunViewModel.CurrentMagazineCount = GunModel.CurrentRoundsInMagazine;
            GunModel.GunViewModel.GunClass = GunModel.GunData.GunClass;
        }

        public override void Uninitialize() { }
    }
}