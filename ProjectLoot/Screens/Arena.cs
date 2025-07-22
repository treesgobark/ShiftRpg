using ANLG.Utilities.Core;
using ProjectLoot.Entities;
using ProjectLoot.Factories;

namespace ProjectLoot.Screens
{
    public partial class Arena
    {
        private void CustomInitialize()
        {
            SummonWaveButton.ButtonPushed += SpawnNextWave;
        }

        private void CustomActivity(bool firstTimeCalled)
        {
            
        }

        private void CustomDestroy()
        {
            
        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {
            
        }

        private void SpawnNextWave()
        {
            foreach (Spawner spawner in SpawnerList)
            {
                var dot = DotFactory.CreateNew(spawner.Position);
                dot.IsBig = Random.Shared.NextBool();
            }
        }
    }
}
