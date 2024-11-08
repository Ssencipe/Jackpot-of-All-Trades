using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSelector : MonoBehaviour
{
    public Sprite[] sprites; // Array of sprites on wheel
    public Button m_SpinButton; // Button to begin spin
    public Button m_LockButton; // Button to lock wheels in place

    public float stopTime = 4; // Duration of wheel spin
    public bool isLocked = false; // Whether a wheel is locked or not
    private int currentSprite = 0; // The index of the current sprite displayed in array
    private float timeSinceLastUpdate = 0f; // A timer used to determine when an update needs to occur for the sprite to change
    private float scrollCooldown = 0.2f; // How often the sprites should change

    private LockManager lockManager; // Reference to the LockManager

    // Sets sprites and checks for button presses
    void Start()
    {
        // Dynamically find the LockManager component in the scene
        lockManager = FindObjectOfType<LockManager>();

        if (lockManager == null)
        {
            Debug.LogError("LockManager not found in the scene! Please ensure there's a LockManager object.");
        }

        // Add listeners for buttons
        m_LockButton.onClick.AddListener(ToggleLock);
        m_SpinButton.onClick.AddListener(StartSpin);
    }

    // Calls for the sprite change constantly while spinning
    void Update()
    {
        ImageSpin();
    }

    // If cooldown is up and wheel is not locked, changes the sprite to the next one in array
    void ImageSpin()
    {
        timeSinceLastUpdate += Time.deltaTime;

        if (timeSinceLastUpdate > scrollCooldown)
        {
            if (!isLocked)
            {
                gameObject.GetComponent<UnityEngine.UI.Image>().sprite = sprites[currentSprite];
                currentSprite++;
                if (currentSprite >= sprites.Length)
                {
                    currentSprite = 0;
                }
            }

            timeSinceLastUpdate = 0f;
        }
    }

    // Stops spin state of wheel
    void EndSpin()
    {
        enabled = false;
    }

    // Enables and starts spin state of wheel for set duration
    public void StartSpin()
    {
        enabled = true;
        Invoke("EndSpin", stopTime);
    }

    // Toggle lock state via LockManager
    void ToggleLock()
    {
        if (lockManager != null)
        {
            lockManager.ToggleLock(this); // Call the LockManager to handle locking/unlocking
        }
    }

    // Restored LockState() method that toggles the lock state of the wheel
    public void LockState()
    {
        isLocked = !isLocked;  // Toggle the lock state
        if (isLocked)
        {
            gameObject.GetComponent<Image>().color = Color.gray; // Optional: visual feedback, e.g., graying out locked wheel
            m_SpinButton.interactable = false;  // Disable spin button when locked
        }
        else
        {
            gameObject.GetComponent<Image>().color = Color.white; // Optional: revert visual feedback when unlocked
            m_SpinButton.interactable = true;  // Enable spin button when unlocked
        }
    }
}
