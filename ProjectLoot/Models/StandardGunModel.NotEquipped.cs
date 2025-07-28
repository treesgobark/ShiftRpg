using ANLG.Utilities.Core;
using ANLG.Utilities.States;

namespace ProjectLoot.Models;

partial class StandardGunModel
{
    private class NotEquipped : TimedState
    {
        private readonly IReadonlyStateMachine _states;
        private StandardGunModel GunModel { get; }

        public NotEquipped(IReadonlyStateMachine states, ITimeManager timeManager, StandardGunModel gunModel)
            : base(timeManager)
        {
            _states = states;
            GunModel     = gunModel;
        }
    
        protected override void AfterTimedStateActivate() { }

        public override IState? EvaluateExitConditions()
        {
            if (GunModel.IsEquipped)
            {
                return _states.Get<Ready>();
            }
        
            return null;
        }

        protected override void AfterTimedStateActivity() { }

        public override void BeforeDeactivate()
        {
            GunModel.GunViewModel.MaximumMagazineCount = GunModel.GunData.MagazineSize;
            GunModel.GunViewModel.CurrentMagazineCount = GunModel.CurrentRoundsInMagazine;
            GunModel.GunViewModel.GunClass = GunModel.GunData.GunClass;
        }
    }
}