using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpinResultManager : MonoBehaviour
{
    public List<string> finalSpriteNames = new List<string>();
    public bool hasClearedResults = false;

    public void AddResult(string spriteName)
    {
        finalSpriteNames.Add(spriteName);
        Debug.Log($"Added {spriteName} to results.");
    }

    public void ClearResults()
    {
        finalSpriteNames.Clear();
        hasClearedResults = false;  // Reset flag for the next spin cycle
        Debug.Log("Cleared all spin results.");
    }
    public void OnSpinComplete()
    {
        // Existing logic to process results
        FindObjectOfType<BattleFlowManager>().OnSlotSpinComplete();
    }
}
