using ANLG.Utilities.Core;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using FlatRedBall;
using Microsoft.Xna.Framework;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Controllers.ModularStates;

namespace ProjectLoot.Entities
{
    public partial class Corpse
    {
        private Vector3 StoredVelocity { get; set; }
        private DurationModule HitstopDuration { get; set; }
        private bool _hasSetVelocity;
        
        private static TimeSpan SlideDuration => TimeSpan.FromMilliseconds(1500);
        private static int Pops => 4;
            
        private float NormalizedProgress => (float)(TimeSinceInitialize / SlideDuration);
        private int GoalPopsHandled => Math.Clamp((int)(NormalizedProgress * Pops) + 1, 0, Pops);
        private TimeSpan TimeSinceInitialize => FrbTimeManager.Instance.TotalGameTime - _creationTime;
        
        private int PopsHandled { get; set; }
        
        private TimeSpan _creationTime;
        
        /// <summary>
        /// Initialization logic which is executed only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
        private void CustomInitialize()
        {
            _creationTime = FrbTimeManager.Instance.TotalGameTime;
            ExplosionSpriteInstance.Visible = false;
        }

        private void CustomActivity()
        {
            HitstopDuration.CustomActivity();
            if (HitstopDuration is not { HasDurationCompleted: true })
            {
                return;
            }

            if (!_hasSetVelocity)
            {
                Velocity        = StoredVelocity;
                _hasSetVelocity = true;
            }
                
            HandlePops();
            
            Velocity *= MathF.Pow(0.1f, TimeManager.SecondDifference);
        }

        private void HandlePops()
        {
            if (PopsHandled < GoalPopsHandled)
            {
                ExplosionSpriteInstance.CurrentFrameIndex = 0;
                ExplosionSpriteInstance.Visible           = true;
                if (PopsHandled != Pops - 1)
                {
                    GlobalContent.GravelHitBigA.Play(0.3f, Random.Shared.NextSingle(-0.2f, 0.2f), 0);
                }
                PopsHandled++;
            }
            
            if (PopsHandled == Pops)
            {
                BodySpriteInstance.Visible = false;
                GlobalContent.Saiga12SingleShot1mSide.Play(0.5f, Random.Shared.NextSingle(-0.2f, 0.2f), 0);
                PopsHandled++;
            }

            if (ExplosionSpriteInstance.JustCycled)
            {
                ExplosionSpriteInstance.Visible = false;

                if (PopsHandled > Pops)
                {
                    Destroy();
                }
            }
        }

        private void CustomDestroy()
        {
        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {
            
        }

        public void InitializeFromEntity(ICorpseInformationComponent corpseInformation, ITransformComponent transformComponent)
        {
            BodySpriteInstance.AnimationChains       = corpseInformation.BodyAnimationChains;
            BodySpriteInstance.CurrentChainName      = corpseInformation.BodyChainName;
            ExplosionSpriteInstance.AnimationChains  = corpseInformation.ExplosionAnimationChains;
            ExplosionSpriteInstance.CurrentChainName = corpseInformation.ExplosionChainName;
            StoredVelocity                           = transformComponent.Velocity;
            HitstopDuration = new DurationModule(FrbTimeManager.Instance, corpseInformation.HitstopDuration);
        }
    }
}
