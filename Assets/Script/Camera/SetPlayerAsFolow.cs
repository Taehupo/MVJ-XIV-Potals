using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class SetPlayerAsFolow : MonoBehaviour
{
    void Awake()
    {
        // get VCamera
        CinemachineVirtualCameraBase vCamera = gameObject.GetComponent<CinemachineVirtualCameraBase>();

        // get player transform
        Transform playerRef = GameObject.Find("Player").transform;

        // set VCamera folow target
        if (vCamera != null)
            vCamera.Follow = playerRef;
    }
}
