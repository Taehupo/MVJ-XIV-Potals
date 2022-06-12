using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RessourcesCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI javelinAmmoText;

    // Start is called before the first frame update
    void Start()
    {
        UpdateJavelinAmmo();
        CharacterManager.Instance.onJavelinAmmoChange += UpdateJavelinAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateJavelinAmmo()
    {
        if (CharacterManager.Instance.MaxAmmo > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).gameObject.SetActive(true);
            javelinAmmoText.text = CharacterManager.Instance.currentJavelinAmmo.ToString();
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
