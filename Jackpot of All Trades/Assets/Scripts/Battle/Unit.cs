using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int unitLevel;

    public int damage;
    public int maxHP;
    public int currentHP;

    public int currentShield;

    public bool TakeDamage(int dmg, BattleHUD hud)
    {
        if (currentShield > 0)
        {
            int remainingDamage = dmg - currentShield;
            currentShield -= dmg;

            if (currentShield < 0)
                currentShield = 0; // Ensure shield doesn’t go negative

            // Update shield UI
            hud.SetShield(currentShield);

            if (remainingDamage > 0)
            {
                currentHP -= remainingDamage; //Overflow damage affects shield
            }
        }
        else
        {
            currentHP -= dmg;
        }

        // Return true if unit is dead
        return currentHP <= 0;
    }


    public void Heal(int amount, BattleHUD hud)
    {
        currentHP += amount;
        if (currentHP > maxHP)
            currentHP = maxHP;

        // Pass the correct position (e.g., the HUD's transform position)
        Vector3 spawnPosition = hud.transform.position;
        hud.SpawnFloatingNumber(amount, true, spawnPosition);
    }

    public void GainShield(int amount)
    {
        currentShield += amount;
    }
}
