using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class PieManager : MonoBehaviour
{
    private List<Wedge> pieWedges;
    public GameObject wedgePrefab;
    public Transform legendPos;
    public GameObject legendPrefab;

    public bool LoadSlow = false;
    private float total = 0;
    int wedgeCounter = 0;

    public class Wedge
    {
        public float fill;
        public string name;
        public Image img;
        public Color col;

        public Wedge(float fill, string name, Image img, Color col)
        {
            this.fill = fill;
            this.name = name;
            this.img = img;
            this.col = col;
        }
    }

    private void Start()
    {
        UnityEngine.Random.InitState(DateTime.Now.Millisecond);

        pieWedges = new List<Wedge>();
        LoadWedges();
        wedgeCounter = pieWedges.Count - 1;

        if (LoadSlow)
            StartCoroutine(SlowLoadPieChart(1, 0));
        else
        {
            UpdateAllWedges();
        }
    }

    private void LoadWedges()
    {
        // Load the CSV file from the Resources folder
        TextAsset dataTextAsset = Resources.Load<TextAsset>("data");
        string data = dataTextAsset.text;

        // Split the data into lines
        string[] splitData = data.Split('\n');

        // Skip the header row
        var newArray = new string[splitData.Length - 1];
        Array.Copy(splitData, 1, newArray, 0, newArray.Length);

        // Iterate through each line
        foreach (string info in newArray)
        {
            string trimmedLine = info.Trim(); // Remove leading/trailing whitespace

            // Skip blank or malformed lines
            if (string.IsNullOrWhiteSpace(trimmedLine)) continue;

            string[] values = trimmedLine.Split(',');

            if (values.Length < 2)
            {
                Debug.LogWarning($"Skipping malformed line: {trimmedLine}");
                continue;
            }

            // Parse the name and value
            string name = values[0];
            if (!float.TryParse(values[1], out float fillVal))
            {
                Debug.LogWarning($"Invalid value for fill: {values[1]}");
                continue;
            }

            // Instantiate the wedge prefab
            GameObject wedgeObj = Instantiate(wedgePrefab);
            wedgeObj.transform.SetParent(transform);
            wedgeObj.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            wedgeObj.SetActive(false);

            Image img = wedgeObj.GetComponent<Image>();
            Color col = new Color(UnityEngine.Random.Range(0.0f, 1f), UnityEngine.Random.Range(0.0f, 1f), UnityEngine.Random.Range(0.0f, 1f));

            // Create and configure the Wedge
            Wedge wedge = new Wedge(fillVal, name, img, col);
            img.color = col;

            total += fillVal;
            pieWedges.Add(wedge);

            // Instantiate the legend prefab
            GameObject legendObj = Instantiate(legendPrefab);
            legendObj.transform.SetParent(legendPos);
            legendObj.GetComponent<Image>().color = col;

            // Ensure the legend displays the name
            var legendText = legendObj.GetComponentInChildren<Text>();
            if (legendText != null)
            {
                legendText.text = name;
            }
        }
    }


    private void UpdateAllWedges()
    {
        float prevZ = 0;

        for(int i = pieWedges.Count-1; i > 0; i--) 
        { 
            prevZ = UpdateSpecificWedge(i, prevZ);
        }
    }

    private float UpdateSpecificWedge(int wedgeIndex, float prevZ = 0)
    {
        Wedge wedge = pieWedges[wedgeIndex];
        float zRot = _ZRotCalculator(prevZ, wedge.fill);
        prevZ = zRot;
        Vector3 rot = wedge.img.transform.eulerAngles;
        rot.z += zRot;

        wedge.img.transform.eulerAngles = rot;
        Text txt = wedge.img.GetComponentInChildren<Text>();

        if (txt != null)
        {
            RectTransform textRect = txt.GetComponent<RectTransform>();
            textRect.anchoredPosition = Vector2.zero; // Center the text
            textRect.localRotation = Quaternion.identity; // Reset rotation
        }

        wedge.img.fillAmount = wedge.fill / total;
        wedge.img.transform.SetAsFirstSibling();

        if (zRot > 90)
        {
            Vector3 rotation = txt.transform.localEulerAngles;
            rotation.z *= -1;
            txt.transform.localEulerAngles = rotation;
        }

        txt.text = (wedge.img.fillAmount * total).ToString();
        txt.color = wedge.col;
        wedge.img.gameObject.SetActive(true);
        return prevZ;
    }

    private IEnumerator SlowLoadPieChart(int waitTime, float prevZ)
    {
        if (wedgeCounter < 0)
            yield break;
        yield return new WaitForSeconds(waitTime);
        prevZ = UpdateSpecificWedge(wedgeCounter, prevZ);
        wedgeCounter--;
        StartCoroutine(SlowLoadPieChart(waitTime, prevZ));
    }

    private float _ZRotCalculator(float prevZ, float fillAmount)
    {
        return (360 * (fillAmount / ((float)(total)))) + prevZ;
    }
}
