using UnityEngine;
//using Systems.Collections;
//using Systems.Collections.Generic;
using UnityEngine.SceneManagement;
public class BattleFlowManager : MonoBehaviour
{
    public GameObject battleScreen;       // Reference to the main battle UI
    public GameObject slotMechanicScreen; // Reference to the slot mechanic UI
    public SpriteSelector[] slotWheels;   // All slot wheels
    public SpinResultManager spinResultManager; // Result manager for spin
    public LockManager lockManager;      // Lock manager for slot locking

    private enum BattlePhase { Start, PlayerTurn, SlotSpin, EnemyTurn, End }
    private BattlePhase currentPhase;

    void Start()
    {
        currentPhase = BattlePhase.Start;
        battleScreen.SetActive(true);
        slotMechanicScreen.SetActive(false);
    }

    public void OnPlayerAction()
    {
        if (currentPhase != BattlePhase.PlayerTurn) return;

        Debug.Log("Player starts slot spin!");
        battleScreen.SetActive(false);
        slotMechanicScreen.SetActive(true);
        foreach (var wheel in slotWheels)
        {
            wheel.enabled = true;
            wheel.StartSpin();
        }
        currentPhase = BattlePhase.SlotSpin;
    }

    public void OnSlotSpinComplete()
    {
        Debug.Log("Slot spin complete, processing results...");
        battleScreen.SetActive(true);
        slotMechanicScreen.SetActive(false);

        // Process spin results
        foreach (var wheel in slotWheels)
        {
            wheel.enabled = false; // Disable spin after it completes
        }

        currentPhase = BattlePhase.EnemyTurn;
        StartEnemyTurn();
    }

    private void StartEnemyTurn()
    {
        Debug.Log("Enemy Turn!");
        // Simulate enemy action (add your own logic here)
        Invoke("EndEnemyTurn", 2f); // Simulate 2 seconds of enemy action
    }

    private void EndEnemyTurn()
    {
        Debug.Log("Enemy turn ends, back to player turn.");
        currentPhase = BattlePhase.PlayerTurn;
    }

    public void EndBattle()
    {
        Debug.Log("Battle Ended!");
        currentPhase = BattlePhase.End;
        // Add end battle logic, such as victory/defeat
    }
}
