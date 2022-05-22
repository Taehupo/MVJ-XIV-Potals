using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeController : MonoBehaviour
{
    #region Members

    public ECharacterShape CharacterShape { get; private set; }

    public static Action<ECharacterShape> OnShapeChanged;

    #endregion


    #region Public Manipulators
    
    /// <summary>
    /// set CharacterShape add call OnShapeChanged
    /// </summary>
    /// <param name="shape"></param>
    public void ChangeShape(ECharacterShape shape)
    {
        SetShape(shape);
    }

    #endregion


    #region Inherited Manipulators

    #endregion


    #region Private Manipulators

    /// <summary>
    /// set CharacterShape add call OnShapeChanged
    /// </summary>
    /// <param name="shape"></param>
    private void SetShape(ECharacterShape shape)
    {
        CharacterShape = shape;
        OnShapeChanged?.Invoke(shape);
    }

    #endregion
}
