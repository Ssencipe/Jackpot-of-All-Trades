using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public Text nameText;
    public Text levelText;
    public Slider hpSlider;

    public GameObject floatingNumberPrefab; //For damage/heal number effect
    public void SetHUD(Unit unit)
    {
        nameText.text = unit.unitName;
        levelText.text = "Lvl " + unit.unitLevel;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;
    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }

    public void SpawnFloatingNumber(int value, bool isHealing, Vector3 spawnPosition)
    {
        GameObject floatingNumber = Instantiate(floatingNumberPrefab, spawnPosition, Quaternion.identity, transform);
        FloatingNumberController controller = floatingNumber.GetComponent<FloatingNumberController>();
        controller.Initialize(value, isHealing);
    }

}
