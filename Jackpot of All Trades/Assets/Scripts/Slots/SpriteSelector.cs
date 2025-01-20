using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSelector : MonoBehaviour
{
    public Sprite[] sprites;
    public Button m_SpinButton;
    public Button m_LockButton;
    public LockManager lockManager;
    public LockTuah lockTuahScript;
    public SpinResultManager spinResultManager;
    public GameObject upperSprite;
    public GameObject lowerSprite;
    public Button m_nudgeUpButton;
    public Button m_nudgeDownButton;
    public NudgeManager nudgeManager;


    public float stopTime = 4;
    public bool isLocked = false;
    private int currentSprite = 0;
    private float timeSinceLastUpdate = 0f;
    private float scrollCooldown = 0.2f;


    void Start()
    {
        enabled = false;

        m_SpinButton.onClick.AddListener(StartSpin);
        m_LockButton.onClick.RemoveAllListeners();  // Prevent redundant listeners
        m_LockButton.onClick.AddListener(() => lockManager.ToggleLock(this));
        m_nudgeUpButton.onClick.AddListener(NudgeUp);
        m_nudgeDownButton.onClick.AddListener(NudgeDown);

        currentSprite = Random.Range(0, sprites.Length);
        gameObject.GetComponent<Image>().sprite = sprites[currentSprite];
        upperSprite.GetComponent<Image>().sprite = sprites[(currentSprite - 1 + sprites.Length) % sprites.Length];
        lowerSprite.GetComponent<Image>().sprite = sprites[(currentSprite + 1) % sprites.Length];
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
            // Update the current image before incrementing currentSprite
            gameObject.GetComponent<Image>().sprite = sprites[currentSprite];

            // Calculate upper and lower sprites based on the current sprite
            upperSprite.GetComponent<Image>().sprite = sprites[(currentSprite - 1 + sprites.Length) % sprites.Length];
            lowerSprite.GetComponent<Image>().sprite = sprites[(currentSprite + 1) % sprites.Length];

            // Increment currentSprite for the next spin
            currentSprite = (currentSprite + 1) % sprites.Length;

            timeSinceLastUpdate = 0f;
        }
    }


    public void StartSpin()
    {
        if (spinResultManager != null && !spinResultManager.hasClearedResults)
        {
            spinResultManager.ClearResults();
            spinResultManager.hasClearedResults = true;
        }

        if (nudgeManager != null)
            nudgeManager.ResetNudges(); // Reset nudges before starting the spin

        StartCoroutine(SpinCoroutine());
    }

    private IEnumerator SpinCoroutine()
    {
        enabled = true;
        yield return new WaitForSeconds(Random.Range(stopTime, stopTime + 2));
        EndSpin();
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
    public void NudgeUp()
    {
        if (FindObjectOfType<NudgeManager>().UseNudge())
        {
            currentSprite = (currentSprite + 1) % sprites.Length;
            UpdateSpriteVisuals();
        }
    }

    public void NudgeDown()
    {
        if (FindObjectOfType<NudgeManager>().UseNudge())
        {
            currentSprite = (currentSprite - 1 + sprites.Length) % sprites.Length;
            UpdateSpriteVisuals();
        }
    }

    private void UpdateSpriteVisuals()
    {
        gameObject.GetComponent<Image>().sprite = sprites[currentSprite];
        upperSprite.GetComponent<Image>().sprite = sprites[(currentSprite - 1 + sprites.Length) % sprites.Length];
        lowerSprite.GetComponent<Image>().sprite = sprites[(currentSprite + 1) % sprites.Length];
    }

}