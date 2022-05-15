using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    #region Members

    public static CharacterManager Instance { get; private set; }


    //private MovementController MovementController { get; private set; }
    //private AttackController AttackController { get; private set; }
    //private ShapeController ShapeController { get; private set; }

    #endregion


    #region Public Manipulators

    #endregion


    #region Inherited Manipulators

    void Awake()
    {
        // do not keep multiple instances
        if (Instance != null)
        {
            Debug.LogError("CharacterManager.Awake() Error - Multiple CharacterManager : " + gameObject.name + " " + Instance.gameObject.name);
            Destroy(gameObject);
            return;
        }

        Instance = this;

        CreateSubComponents();
    }

    #endregion


    #region Private Manipulators

    void CreateSubComponents()
    {

    }

    #endregion
}
