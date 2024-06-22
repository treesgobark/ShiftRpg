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
        
        void OnEnemyVsEnemyCollided (Entities.Enemy enemy, Entities.Enemy enemy2) 
        {
            switch (enemy, enemy2)
            {
                case (TargetDummy, not TargetDummy):
                    enemy.CollideAgainstMove(enemy2, 1, 0);
                    break;
                case (not TargetDummy, TargetDummy):
                    enemy.CollideAgainstMove(enemy2, 0, 1);
                    break;
                case (not TargetDummy, not TargetDummy):
                    enemy.CollideAgainstMove(enemy2, 1, 1);
                    break;
                default:
                    enemy.CollideAgainstMove(enemy2, 0, 0);
                    break;
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
            // if (meleeHitbox.)
            // {
            //     
            // }
            
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
