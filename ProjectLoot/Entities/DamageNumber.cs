using ANLG.Utilities.Core.Constants;
using ANLG.Utilities.FlatRedBall.Extensions;
using FlatRedBall;
using Microsoft.Xna.Framework;
using ProjectLoot.Effects;
using RenderingLibrary.Graphics;

namespace ProjectLoot.Entities
{
    public partial class DamageNumber
    {
        private SourceTag Source { get; set; }
        private bool HasPlayedSound { get; set; } = false;
        
        /// <summary>
        /// Initialization logic which is executed only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
        private void CustomInitialize()
        {
            DamageNumberRuntimeInstance.TextInstance.OverrideTextRenderingPositionMode =
                TextRenderingPositionMode.FreeFloating;
            DamageNumberRuntimeInstance.TextInstance.FontScale = 0.75f;
            Velocity = 50f * Vector3.UnitX.RandomizeAngleBetween(MathConstants.EighthTurn, 3 * MathConstants.EighthTurn)
                .RandomizeMagnitudeBetween(0.5f, 1f);
        }

        private void CustomActivity()
        {
            DamageNumberRuntimeInstance.TextAlpha -= (int)(400f * TimeManager.SecondDifference);
            if (DamageNumberRuntimeInstance.TextAlpha <= 0)
            {
                Destroy();
            }

            if (!HasPlayedSound)
            {
                if (Source == SourceTag.Melee)
                {
                    GlobalContent.SwordImpact.Play(0.1f, 0f, 0f);
                }
                else if (Source == SourceTag.Gun)
                {
                    HitMarker.Play(0.1f, 0f, 0f);
                }

                HasPlayedSound = true;
            }
        }

        private void CustomDestroy()
        {


        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {


        }

        public void SetStartingValues(string text, float fontScale, Vector3 position, SourceTag source)
        {
            DamageNumberRuntimeInstance.Text                   = text;
            DamageNumberRuntimeInstance.TextInstanceFont_Scale = fontScale;
            Position                                           = position;
            Source                                             = source;
        }
    }
}
