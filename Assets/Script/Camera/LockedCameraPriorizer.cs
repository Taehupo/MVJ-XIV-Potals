using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// edit Parent VirtualCamera Priority if the player is within the trigger area
/// </summary>
public class LockedCameraPriorizer : MonoBehaviour
{
    [SerializeField]
    private int m_PriorityOnTriggered = 100;
    private int m_BasePriority = 10;
    private CinemachineVirtualCameraBase m_Camera;

    private void Awake()
    {
        // get vcamera and its base Priority
        m_Camera = gameObject.transform.parent.GetComponentInChildren<CinemachineVirtualCameraBase>();
        if(m_Camera == null)
        {
            Debug.LogError("LockedCameraPriorizer.Awake() Error : gameObject has no child /  with CinemachineVirtualCamera " + gameObject.name);
            return;
        }

        m_BasePriority = m_Camera.m_Priority;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if player set vcamera priority
        if (other.name == "Player")
        {
            if (m_Camera != null)
                m_Camera.m_Priority = m_PriorityOnTriggered;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // if player reset vcamera priority
        if(other.name == "Player")
        {
            if (m_Camera != null)
                m_Camera.m_Priority = m_BasePriority;
        }
    }
}
