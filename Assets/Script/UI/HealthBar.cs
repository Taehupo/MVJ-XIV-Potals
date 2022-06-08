using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Sprite fullHealth;
    [SerializeField] private Sprite halfHealth;
    [SerializeField] private Sprite emptyHealth;

    private List<Image> healthIcons = new List<Image>();

    // Start is called before the first frame update
    void Start()
    {
        UpdateMaxHealth();
        UpdateCurrentHealth();
        CharacterManager.Instance.HealthManager.onHealthChanged += UpdateCurrentHealth;
        CharacterManager.Instance.HealthManager.onMaxHealthChanged += UpdateMaxHealth;
        //CharacterManager.Instance.HealthManager.onHeal += UpdateCurrentHealth;
    }

    private void AddIcon()
    {
        GameObject icon = new GameObject("HealthIcon", typeof(Image));
        icon.transform.SetParent(transform, false);
        //icon.transform.parent = transform;
        icon.transform.localPosition = Vector3.zero;
        healthIcons.Add(icon.GetComponent<Image>());
    }

    private void RemoveIcon()
    {
        Image icon = healthIcons[healthIcons.Count];
        healthIcons.RemoveAt(healthIcons.Count);
        Destroy(icon);
    }

    private void UpdateMaxHealth()
    {
        int numberOfIcons = CharacterManager.Instance.HealthManager.GetMaxHealth() / 2;
        while (healthIcons.Count < numberOfIcons)
            AddIcon();
        while (healthIcons.Count > numberOfIcons)
            RemoveIcon();
        UpdateCurrentHealth();
    }

    private void UpdateCurrentHealth()
    {
        int i = 0;
        // Set full hearts
        while (i < CharacterManager.Instance.HealthManager.GetHealth() / 2)
        {
            healthIcons[i].sprite = fullHealth;
            i++;
        }
        // Set half heart
        if (CharacterManager.Instance.HealthManager.GetHealth()%2 == 1)
        {
            healthIcons[i].sprite = halfHealth;
            i++;
        }
        // Set empty hearts
        while (i < CharacterManager.Instance.HealthManager.GetMaxHealth() / 2)
        {
            healthIcons[i].sprite = emptyHealth;
            i++;
        }
    }
}
