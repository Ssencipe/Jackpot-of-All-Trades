using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSelector : MonoBehaviour
{
    public Sprite[] sprites;
    public Button m_SpinButton;
    public Button m_LockButton;
    private LockManager lockManager;
    public LockTuah lockTuahScript;

    public float stopTime = 4;
    public bool isLocked = false;
    private int currentSprite = 0;
    private float timeSinceLastUpdate = 0f;
    private float scrollCooldown = 0.2f;

    private SpinResultManager spinResultManager;

    void Start()
    {
        enabled = false;
        lockManager = FindObjectOfType<LockManager>();
        spinResultManager = FindObjectOfType<SpinResultManager>();

        if (lockManager == null) Debug.LogError("LockManager not found in the scene!");
        if (spinResultManager == null) Debug.LogError("SpinResultManager not found in the scene!");

        m_SpinButton.onClick.AddListener(StartSpin);
        m_LockButton.onClick.RemoveAllListeners();  // Prevent redundant listeners
        m_LockButton.onClick.AddListener(() => lockManager.ToggleLock(this));

        currentSprite = Random.Range(0, sprites.Length);
        gameObject.GetComponent<Image>().sprite = sprites[currentSprite];
    }

    void Update()
    {
        if (!isLocked) ImageSpin();
    }

    void ImageSpin()
    {
        timeSinceLastUpdate += Time.deltaTime;
        if (timeSinceLastUpdate > scrollCooldown)
        {
            gameObject.GetComponent<Image>().sprite = sprites[currentSprite];
            currentSprite = (currentSprite + 1) % sprites.Length;
            timeSinceLastUpdate = 0f;
        }
    }

    public void StartSpin()
    {
        if (spinResultManager != null && !spinResultManager.hasClearedResults)
        {
            spinResultManager.ClearResults();  // Clear previous spin results once
            spinResultManager.hasClearedResults = true;
        }

        enabled = true;
        Invoke("EndSpin", Random.Range(stopTime, stopTime + 2));
    }

    void EndSpin()
    {
        enabled = false;
        if (spinResultManager != null && sprites.Length > 0)
        {
            string finalSpriteName = sprites[currentSprite].name;
            spinResultManager.AddResult(finalSpriteName);
        }
    }
}
