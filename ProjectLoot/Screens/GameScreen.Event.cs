using FlatRedBall.Math.Geometry;
using FlatRedBall.TileCollisions;
using ProjectLoot.DataTypes;
using ProjectLoot.Effects;
using ProjectLoot.Entities;

namespace ProjectLoot.Screens
{
    public partial class GameScreen
    {
        void OnPlayerVsEnemyCollided(Player player, Enemy enemy) 
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
        
        void OnEnemyVsEnemyCollided(Enemy enemy, Enemy enemy2) 
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
                case (TargetDummy, TargetDummy):
                    enemy.CollideAgainstMove(enemy2, 1, 1);
                    break;
            }
        }
        
        void OnPlayerVsProjectileCollided(Player player, Projectile projectile)
        {
            if (!projectile.IsActive) { return; }
            if (!projectile.AppliesTo.Contains(player.EffectsComponent.Team)) { return; }
            
            player.EffectsComponent.Handle(projectile.OnHitTargetEffects);
            projectile.HolderEffectsComponent.Handle(projectile.OnHitHolderEffects);
            
            projectile.Destroy();
        }
        
        void OnProjectileVsEnemyCollided(Projectile projectile, Enemy enemy) 
        {
            if (!projectile.IsActive) { return; }
            if (!projectile.AppliesTo.Contains(enemy.Effects.Team)) { return; }
            
            enemy.Effects.Handle(projectile.OnHitTargetEffects);
            projectile.HolderEffectsComponent.Handle(projectile.OnHitHolderEffects);
            
            projectile.Destroy();
        }
        
        void OnProjectileVsSolidCollisionCollided(Projectile projectile, TileShapeCollection tileShapeCollection) 
        {
            projectile.Destroy();
        }
        
        void OnMeleeHitboxVsPlayerCollided(MeleeHitbox meleeHitbox, Player player)
        {
            if (!meleeHitbox.AppliesTo.Contains(player.EffectsComponent.Team)) { return; }
            if (!meleeHitbox.IsActive) { return; }
            
            player.EffectsComponent.Handle(meleeHitbox.TargetHitEffects);
            meleeHitbox.HolderEffectsComponent.Handle(meleeHitbox.HolderHitEffects);
        }
        
        void OnMeleeHitboxVsEnemyCollided(MeleeHitbox meleeHitbox, Enemy enemy) 
        {
            if (!meleeHitbox.AppliesTo.Contains(enemy.Effects.Team)) { return; }
            if (!meleeHitbox.IsActive) { return; }
            
            enemy.Effects.Handle(meleeHitbox.TargetHitEffects);
            meleeHitbox.HolderEffectsComponent.Handle(meleeHitbox.HolderHitEffects);
        }
        
        void OnPlayerVsWeaponDropCollided(Player player, WeaponDrop weaponDrop)
        {
            weaponDrop.ShowPreview();

            if (player.GameplayInputDevice.Interact.WasJustPressed)
            {
                bool wasPickedUp;
                if (weaponDrop.ContainedWeapon is GunData gunData)
                {
                    wasPickedUp = player.PickUpWeapon(gunData);
                }
                else if (weaponDrop.ContainedWeapon is MeleeWeaponData meleeWeaponData)
                {
                    wasPickedUp = player.PickUpWeapon(meleeWeaponData);
                }
                else
                {
                    throw new InvalidOperationException("not sure how bad data got in here");
                }

                if (wasPickedUp)
                {
                    weaponDrop.Destroy();
                }
            }
        }
        
        void OnMeleeHitboxVsButtonCollided (Entities.MeleeHitbox meleeHitbox, Entities.Button button) 
        {
            if (!meleeHitbox.AppliesTo.Contains(button.Effects.Team)) { return; }
            if (!meleeHitbox.IsActive) { return; }
            
            button.Effects.Handle(meleeHitbox.TargetHitEffects);
            meleeHitbox.HolderEffectsComponent.Handle(meleeHitbox.HolderHitEffects);
        }

    }
}
