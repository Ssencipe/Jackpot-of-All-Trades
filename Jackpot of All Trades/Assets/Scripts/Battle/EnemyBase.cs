using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public string enemyName;
    public int maxHP;
    public int currentHP;
    public int damage;
    public int currentShield;

    protected BattleHUD hud; // Keep track of this enemy's HUD

    public virtual void Initialize(BattleHUD enemyHUD)
    {
        hud = enemyHUD;
        hud.SetHUD(this); // Set up the enemy’s UI using this instance
    }

    public virtual bool TakeDamage(int dmg)
    {
        if (currentShield > 0)
        {
            int remainingDamage = dmg - currentShield;
            currentShield -= dmg;

            if (currentShield < 0)
                currentShield = 0; // Prevent negative shield

            hud.SetShield(currentShield);

            if (remainingDamage > 0)
            {
                currentHP -= remainingDamage; // Overflow damage hits HP
            }
        }
        else
        {
            currentHP -= dmg;
        }

        hud.SetHP(currentHP);
        return currentHP <= 0; // Returns true if enemy dies
    }

    public virtual void Attack(Unit target, BattleHUD targetHUD)
    {
        target.TakeDamage(damage, targetHUD); // Pass the correct HUD reference
    }

    public virtual void GainShield(int amount)
    {
        currentShield += amount;
        hud.SetShield(currentShield);
    }

    public abstract void PerformAction(Unit player, BattleHUD playerHUD);
}
