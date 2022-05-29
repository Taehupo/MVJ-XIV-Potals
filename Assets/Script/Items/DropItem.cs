using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/DropItem ", order = 1)]
public class DropItem : ScriptableObject
{
    #region Members

    [SerializeField]
    private GameObject m_ItemPrefab;
    [SerializeField]
    [Range(0f, 1f)]
    private float m_DropRate = 0.5f;

    #endregion

    #region Accessors

    public GameObject ItemPrefab { get => m_ItemPrefab; }
    public float DropRate { get => m_DropRate; }

    #endregion
}
