using UnityEngine;
using UnityEngine.UI;

public class LockTuah : MonoBehaviour
{
    public GameObject m_LockObject;  // Lock sprite object

    void Start()
    {
        if (m_LockObject != null)
        {
            m_LockObject.GetComponent<Image>().enabled = false;  // Initially hide the lock sprite
        }
    }

    public void LockedIn(bool isActive)
    {
        if (m_LockObject != null)
        {
            m_LockObject.GetComponent<Image>().enabled = isActive;  // Set visibility based on lock state
        }
    }
}
