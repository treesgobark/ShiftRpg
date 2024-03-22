using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ANLG.Utilities.FlatRedBall.Extensions;
using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Debugging;
using FlatRedBall.Entities;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Graphics.Particle;
using FlatRedBall.Math.Geometry;
using FlatRedBall.Screens;
using Microsoft.Xna.Framework;
using ShiftRpg.Contracts;
using ShiftRpg.Effects;
using ShiftRpg.InputDevices;
using ShiftRpg.Screens;

namespace ShiftRpg.Entities
{
    public abstract partial class Enemy : ITakesShatterDamage
    {
        private int _currentShatterDamage;
        private int _currentHealth;

        /// <summary>
        /// Initialization logic which is executed only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
        private void CustomInitialize()
        {
            var hudParent = gumAttachmentWrappers[0];
            hudParent.ParentRotationChangesRotation = false;
            Team                                    = Team.Enemy;
            CurrentHealth                           = MaxHealth;
            PersistentEffects                       = new List<IPersistentEffect>();
            RecentEffects = new List<(Guid EffectId, double EffectTime)>();
        }

        private void CustomActivity()
        {
            HandlePersistentEffects();
        }

        private void CustomDestroy()
        {


        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {


        }

        public void HandlePersistentEffects()
        {
            List<IEffect> effects = [];

            for (var i = PersistentEffects.Count - 1; i >= 0; i--)
            {
                var effect = PersistentEffects[i];
                if (effect is DamageOverTimeEffect { ShouldApply: true } dot)
                {
                    effects.Add(dot.GetDamageEffect());
                    if (dot.RemainingTicks <= 0)
                    {
                        PersistentEffects.RemoveAt(i);
                    }
                }
            }

            if (effects.Count > 0)
            {
                HandleEffects(effects);
            }
        }

        public virtual void HandleEffects(IReadOnlyList<IEffect> effects)
        {
            foreach (var effect in effects)
            {
                if (RecentEffects.Any(t => t.EffectId == effect.EffectId))
                {
                    continue;
                }
                
                effect.HandleStandardDamage(this)
                    .HandleStandardShatterDamage(this)
                    .HandleStandardApplyShatter(this)
                    .HandleStandardKnockback(this)
                    .HandleStandardPersistentEffect(this);
            }
        }

        public IList<IPersistentEffect> PersistentEffects { get; protected set; }
        public Team Team { get; protected set; }

        public IList<(Guid EffectId, double EffectTime)> RecentEffects { get; protected set; }
        public float CurrentHealthPercentage => 100f * CurrentHealth / MaxHealth;
        public float ShatterSubProgressPercentage => CurrentHealth == 0 ? 100f : 100f * CurrentShatterDamage / CurrentHealth;
        public double TimeSinceLastDamage => TimeManager.CurrentScreenSecondsSince(LastDamageTime);
        public bool IsInvulnerable => TimeSinceLastDamage < InvulnerabilityTimeAfterDamage;

        public int CurrentHealth
        {
            get => _currentHealth;
            set => _currentHealth = (int)MathHelper.Clamp(value, -1, MaxHealth);
        }

        public double LastDamageTime { get; set; }
        public virtual void TakeDamage(int damage)
        {
            CurrentHealth                                       -= damage;
            EnemyHealthBarRuntimeInstance.ProgressPercentage    =  CurrentHealthPercentage;
            if (CurrentHealth <= 0)
            {
                Destroy();
            }
        }

        public int CurrentShatterDamage
        {
            get => _currentShatterDamage;
            set
            {
                _currentShatterDamage = value;
                if (_currentShatterDamage < 0)
                {
                    Console.WriteLine();
                }
            }
        }

        public float MaxShatterDamagePercentage => 20;

        public void TakeShatterDamage(int damage)
        {
            CurrentShatterDamage                                += damage;
            CurrentShatterDamage = Math.Min((int)(MaxShatterDamagePercentage / 100f * MaxHealth), CurrentShatterDamage);
            CurrentShatterDamage = Math.Min(CurrentHealth, CurrentShatterDamage);
            EnemyHealthBarRuntimeInstance.SubProgressPercentage = ShatterSubProgressPercentage;
        }

        public void ResetShatterDamage()
        {
            CurrentShatterDamage                                = 0;
            EnemyHealthBarRuntimeInstance.SubProgressPercentage = ShatterSubProgressPercentage;
        }
    }
}
