using FlatRedBall.Math.Geometry;
using FlatRedBall.TileCollisions;
using ShiftRpg.Entities;

namespace ShiftRpg.Screens
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
            
            projectile.ApplyHolderEffects(projectile.HolderHitEffects);
            player.HandleEffects(projectile.TargetHitEffects);
            projectile.Destroy();
        }
        
        void OnPlayerVsMeleeWeaponCollided (Player player, MeleeWeapon meleeWeapon)
        {
            meleeWeapon.ApplyHolderEffects(meleeWeapon.HolderHitEffects);
            player.HandleEffects(meleeWeapon.TargetHitEffects);
        }
        
        void OnProjectileVsEnemyCollided (Projectile projectile, Enemy enemy) 
        {
            if (!projectile.IsActive) { return; }
            
            projectile.ApplyHolderEffects(projectile.HolderHitEffects);
            enemy.HandleEffects(projectile.TargetHitEffects);
            projectile.Destroy();
        }
        
        void OnMeleeWeaponVsEnemyCollided (MeleeWeapon meleeWeapon, Enemy enemy) 
        {
            meleeWeapon.ApplyHolderEffects(meleeWeapon.HolderHitEffects);
            enemy.HandleEffects(meleeWeapon.TargetHitEffects);
        }
        
        void OnProjectileVsSolidCollisionCollided (Projectile projectile, TileShapeCollection tileShapeCollection) 
        {
            projectile.Destroy();
        }
    }
}
