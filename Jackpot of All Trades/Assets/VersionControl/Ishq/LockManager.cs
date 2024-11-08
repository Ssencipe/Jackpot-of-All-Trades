using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // Include TextMeshPro namespace

public class LockManager : MonoBehaviour
{
    public int maxLocks = 3; // Max number of locks allowed
    public TextMeshProUGUI lockCountText; // Reference to TextMeshProUGUI component for lock count display

    private int currentLocks = 0; // Current number of locked wheels

    public List<SpriteSelector> wheels; // List of all wheels that can be locked/unlocked

    void Start()
    {
        UpdateLockCountText(); // Display initial lock count
    }

    // Method to lock/unlock a wheel, called by each wheel's button
    public void ToggleLock(SpriteSelector wheel)
    {
        if (wheel.isLocked)
        {
            UnlockWheel(wheel);
        }
        else
        {
            LockWheel(wheel);
        }
    }

    void LockWheel(SpriteSelector wheel)
    {
        if (currentLocks < maxLocks) // Only allow locking if there are available locks
        {
            wheel.LockState(); // Lock the wheel
            currentLocks++; // Increase the lock count
            UpdateLockCountText(); // Update the lock count UI
        }
        else
        {
            Debug.Log("Maximum lock limit reached!");
        }
    }

    void UnlockWheel(SpriteSelector wheel)
    {
        wheel.LockState(); // Unlock the wheel
        currentLocks--; // Decrease the lock count
        UpdateLockCountText(); // Update the lock count UI
    }

    void UpdateLockCountText()
    {
        if (lockCountText != null)
        {
            lockCountText.text = "Available Locks: " + (maxLocks - currentLocks);
        }
    }
}
