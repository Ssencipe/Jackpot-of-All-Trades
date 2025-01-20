using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NudgeManager : MonoBehaviour
{
    public int maxNudges = 2;
    private int currentNudges;
    public TextMeshProUGUI nudgeText;

    void Start()
    {
        ResetNudges();
    }

    public void ResetNudges()
    {
        currentNudges = maxNudges;
        UpdateNudgeText();
    }

    public bool UseNudge()
    {
        if (currentNudges > 0)
        {
            currentNudges--;
            UpdateNudgeText();
            return true;
        }
        return false;
    }

    private void UpdateNudgeText()
    {
        nudgeText.text = $"Nudges: {currentNudges}";
    }
}
