using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilumDoor : MonoBehaviour
{

    // 0 for sword, 1 for pilum, 2 for super pilum
    [SerializeField]
    private DoorType securityLevel = DoorType.Normal;

    [SerializeField]
    private EEventFlag associatedFlag = EEventFlag.DoorDiane01Unlocked;

    [SerializeField]
    private Color normalDoorColor = Color.gray;
    [SerializeField]
    private Color pilumDoorColor = Color.blue;
    [SerializeField]
    private Color superPilumDoorColor = Color.green;
    [SerializeField]
    private Color lockedDoorColor = Color.red;

    [SerializeField]
    private float openingSpeed = 2f;
    private bool isOpening = false;

    private void Start()
    {
        UpdateSecurity();
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        switch (securityLevel)
        {
            case DoorType.Normal:
                renderer.color = normalDoorColor;
                break;
            case DoorType.Pilum:
                renderer.color = pilumDoorColor;
                break;
            case DoorType.SuperPilum:
                renderer.color = superPilumDoorColor;
                break;
            case DoorType.Locked:
                renderer.color = lockedDoorColor;
                break;
        }
    }

    private void Update()
    {
        if (isOpening)
        {
            transform.localScale = Vector3.Lerp(transform.localScale,
                new Vector3(transform.localScale.x, 0f, transform.localScale.z), Time.deltaTime * openingSpeed);
            if (transform.localScale.y < 0.1f)
                Destroy(gameObject);
        }
        else
            UpdateSecurity();
    }
    private void UpdateSecurity()
    {
        if (GameManager.instance != null && securityLevel != DoorType.Normal)
        {
            // Check flag on GM
            if (GameManager.instance.activeEventFlags.Contains(associatedFlag))
            {
                securityLevel = DoorType.Normal;
                GetComponent<SpriteRenderer>().color = normalDoorColor;
            }
        }
        
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("coll");
        if (collision.otherCollider.GetComponent<Javelin>() != null)
            GetComponent<HealthManager>().TakeDamage(1);
    }*/
    public void Hit(int hitType)
    {
        if (hitType >= (int)securityLevel)
        {
            GameManager.instance.AddFlag(associatedFlag);
            isOpening = true;
            //Destroy(gameObject);
        }
    }
}
public enum DoorType
{
    Normal,

    Pilum,

    SuperPilum,

    Locked
}