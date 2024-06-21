using FlatRedBall.Debugging;
using FlatRedBall.Math.Geometry;
using FlatRedBall.TileCollisions;
using ProjectLoot.Entities;

namespace ProjectLoot.Screens
{
    public partial class GameScreen
    {
        void OnPlayerVsEnemyCollided (Player player, Enemy enemy) 
        {
            if (enemy is TargetDummy dummy)
            {
                dummy.CollideAgainstMove(player, 1, 0);
            }
            else
            {
                enemy.CollideAgainstMove(player, 1, 1);
            }
        }
        
        void OnPlayerVsProjectileCollided (Player player, Projectile projectile)
        {
            if (!projectile.IsActive) { return; }
            
            player.Effects.Handle(projectile.TargetHitEffects);
            projectile.Holder.Effects.Handle(projectile.HolderHitEffects);
            
            projectile.Destroy();
        }
        
        void OnProjectileVsEnemyCollided (Projectile projectile, Enemy enemy) 
        {
            if (!projectile.IsActive) { return; }
            
            enemy.Effects.Handle(projectile.TargetHitEffects);
            projectile.Holder.Effects.Handle(projectile.HolderHitEffects);
            
            projectile.Destroy();
        }
        
        void OnProjectileVsSolidCollisionCollided (Projectile projectile, TileShapeCollection tileShapeCollection) 
        {
            projectile.Destroy();
        }
        
        void OnMeleeHitboxVsPlayerCollided (Entities.MeleeHitbox meleeHitbox, Entities.Player player) 
        {
            player.Effects.Handle(meleeHitbox.TargetHitEffects);
            meleeHitbox.Holder.Effects.Handle(meleeHitbox.HolderHitEffects);
        }
        
        void OnMeleeHitboxVsEnemyCollided (Entities.MeleeHitbox meleeHitbox, Entities.Enemy enemy) 
        {
            enemy.Effects.Handle(meleeHitbox.TargetHitEffects);
            meleeHitbox.Holder.Effects.Handle(meleeHitbox.HolderHitEffects);
        }

    }
}
