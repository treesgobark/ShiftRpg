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
            projectile.Destroy();
        }
        
        void OnPlayerVsMeleeWeaponCollided (Player player, MeleeWeapon meleeWeapon)
        {
            player.Effects.HandlerCollection.Handle(meleeWeapon.TargetHitEffects);
        }
        
        void OnProjectileVsEnemyCollided (Projectile projectile, Enemy enemy) 
        {
            if (!projectile.IsActive) { return; }
            
            enemy.Effects.Handle(projectile.TargetHitEffects);
            projectile.Destroy();
        }
        
        void OnMeleeWeaponVsEnemyCollided (MeleeWeapon meleeWeapon, Enemy enemy) 
        {
            enemy.Effects.Handle(meleeWeapon.TargetHitEffects);
        }
        
        void OnProjectileVsSolidCollisionCollided (Projectile projectile, TileShapeCollection tileShapeCollection) 
        {
            projectile.Destroy();
        }
    }
}
