using TMPro;
using UnityEngine;
using System.Collections;

public class FloatingNumberController : MonoBehaviour
{
    public TextMeshProUGUI numberText;
    public Color damageColor = Color.red;
    public Color healingColor = Color.green;
    public float moveSpeed = 2f;
    public float lifetime = 1.5f;

    private float alpha;

    public void Initialize(int value, bool isHealing)
    {
        // Set text and color
        numberText.text = value.ToString();
        numberText.color = isHealing ? healingColor : damageColor;

        // Initialize alpha for fading
        alpha = 1f;

        // Start fading after setup
        StartCoroutine(FadeAndDestroy());
    }

    private IEnumerator FadeAndDestroy()
    {
        float elapsedTime = 0f;

        while (elapsedTime < lifetime)
        {
            elapsedTime += Time.deltaTime;

            // Move upward
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

            // Fade out
            alpha = Mathf.Lerp(1f, 0f, elapsedTime / lifetime);
            numberText.color = new Color(numberText.color.r, numberText.color.g, numberText.color.b, alpha);

            yield return null;
        }

        Destroy(gameObject); // Destroy after fade
    }
}
