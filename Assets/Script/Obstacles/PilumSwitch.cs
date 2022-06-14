using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilumSwitch : MonoBehaviour
{

    [SerializeField]
    private EEventFlag associatedFlag = EEventFlag.count;

    private void Start()
    {
        // Check flag on GM
        if (GameManager.instance.activeEventFlags.Contains(associatedFlag))
            Destroy(gameObject);
    }

    // Update is called once per frame
    public void ActivateSwitch()
    {
        GameManager.instance.AddFlag(associatedFlag);
        Destroy(gameObject);
    }
}
