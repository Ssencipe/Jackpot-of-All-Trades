using UnityEngine;
using System.Collections.Generic;

public class WheelManager : MonoBehaviour
{
    public GameObject wheelPrefab; // Assign the SlotsWheel prefab in the Inspector
    public int wheelCount = 5; // Set the desired number of wheels

    private List<GameObject> wheels = new List<GameObject>();
    public float wheelSpacing = 150f; // Adjust spacing between wheels

    public RectTransform panelRectTransform; // Assign the panel in the Inspector

    void Start()
    {
        if (wheelPrefab == null)
        {
            Debug.LogError("Wheel Prefab is not assigned!");
            return;
        }

        if (panelRectTransform == null)
        {
            Debug.LogError("Panel RectTransform is not assigned!");
            return;
        }

        GenerateWheels();
        PositionWheels();
    }

    public void GenerateWheels()
    {
        // Destroy all existing wheels
        foreach (var wheel in wheels)
        {
            Destroy(wheel);
        }
        wheels.Clear();

        // Generate wheels based on the wheelCount
        for (int i = 0; i < wheelCount; i++)
        {
            GameObject newWheel = Instantiate(wheelPrefab, panelRectTransform);
            newWheel.name = "SlotsWheel" + (i + 1);

            // Auto-assign references
            SpriteSelector selector = newWheel.GetComponent<SpriteSelector>();
            if (selector != null)
            {
                selector.Initialize();
            }

            wheels.Add(newWheel);
        }

        Debug.Log(wheelCount + " wheels generated.");
    }

    public void PositionWheels()
    {
        if (wheels.Count == 0)
        {
            Debug.LogError("No wheels to position!");
            return;
        }

        float totalWidth = (wheels.Count - 1) * wheelSpacing;
        float startX = -totalWidth / 2;

        for (int i = 0; i < wheels.Count; i++)
        {
            RectTransform wheelRect = wheels[i].GetComponent<RectTransform>();
            if (wheelRect == null)
            {
                Debug.LogError("Wheel " + wheels[i].name + " is missing RectTransform!");
                continue;
            }

            wheelRect.SetParent(panelRectTransform, false);
            Vector3 newPosition = new Vector3(startX + i * wheelSpacing, 0, 0);
            wheelRect.anchoredPosition = newPosition;
        }

        Debug.Log("Wheels positioned.");
    }

    // Call this method to update wheels dynamically
    public void UpdateWheels(int newWheelCount)
    {
        wheelCount = newWheelCount;
        GenerateWheels();
        PositionWheels();
    }
}
