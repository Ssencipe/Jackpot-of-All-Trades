using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class LockTuah : MonoBehaviour
{
    public Button m_LockButton;
    public GameObject m_LockObject;

    public bool isActive = false;

    void Start()
    {
        m_LockObject.GetComponent<Image>().enabled = !m_LockObject.GetComponent<Image>().enabled;
        m_LockButton.onClick.AddListener(LockedIn);
    }

    public void LockedIn()
    {
        isActive = !isActive;
        m_LockObject.GetComponent<Image>().enabled = !m_LockObject.GetComponent<Image>().enabled;
    }
}