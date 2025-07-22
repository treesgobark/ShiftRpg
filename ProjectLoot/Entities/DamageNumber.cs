using ANLG.Utilities.Core;
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
        private Team Team { get; set; }
        private bool HasPlayedSound { get; set; }
        private float Damage { get; set; }
        
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

            if (Team == Team.Player)
            {
                DamageNumberRuntimeInstance.TextRed = 234;
                DamageNumberRuntimeInstance.TextGreen = 234;
                DamageNumberRuntimeInstance.TextBlue = 80;
            }
            
            if (DamageNumberRuntimeInstance.TextAlpha <= 0)
            {
                Destroy();
            }

            if (!HasPlayedSound)
            {
                float pitch = Random.Shared.NextSingle(-0.2f, 0.2f);
                
                switch (Source)
                {
                    case SourceTag.Sword:
                    case SourceTag.Spear:
                        if (Damage >= 25)
                        {
                            GlobalContent.ShotgunBlastQuick.Play(0.3f, pitch - 0.8f, 0f);
                        }

                        GlobalContent.SwordImpact.Play(0.1f, pitch, 0f);
                        break;
                    case SourceTag.Gun:
                        HitMarker.Play(0.1f, pitch, 0f);
                        break;
                    case SourceTag.Fists:
                        if (Damage >= 16)
                        {
                            GlobalContent.ThudShotLow.Play(0.35f, pitch - 0.8f, 0f);
                        }

                        GlobalContent.FistHitA.Play(0.1f, pitch, 0f);
                        break;
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

        public void SetStartingValues(float damage, float fontScale, Vector3 position, SourceTag source, Team team)
        {
            Damage                                             = damage;
            DamageNumberRuntimeInstance.Text                   = damage.ToString();
            DamageNumberRuntimeInstance.TextInstanceFont_Scale = fontScale;
            Position                                           = position;
            Source                                             = source;
            Team                                               = team;
        }
    }
}
