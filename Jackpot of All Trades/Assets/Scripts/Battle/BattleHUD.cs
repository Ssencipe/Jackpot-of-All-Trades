using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{
    public Text nameText;
    public Text levelText;
    public Slider hpSlider;
    public TextMeshProUGUI shieldText;

    public GameObject floatingNumberPrefab;

    public void SetHUD(Unit unit)
    {
        nameText.text = unit.unitName;
        levelText.text = "Lvl " + unit.unitLevel;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;
        SetShield(unit.currentShield); // Initialize shield display
    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }

    public void SetShield(int shield)
    {
        shieldText.text = "Shield: " + shield; // Update shield display
    }

    public void SpawnFloatingNumber(int value, bool isHealing, Vector3 spawnPosition) //For damage/heal number pop-ups
    {
        GameObject floatingNumber = Instantiate(floatingNumberPrefab, spawnPosition, Quaternion.identity, transform);
        FloatingNumberController controller = floatingNumber.GetComponent<FloatingNumberController>();
        controller.Initialize(value, isHealing);
    }
}
