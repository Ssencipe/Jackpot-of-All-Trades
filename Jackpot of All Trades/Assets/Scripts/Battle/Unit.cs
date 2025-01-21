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

    public bool TakeDamage(int dmg)
    {
        currentHP -= dmg;

        // Return true if the unit is dead
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



}
