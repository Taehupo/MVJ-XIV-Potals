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

    #endregion


    #region Inherited Manipulators

    #endregion


    #region Private Manipulators

    private void SetShape(ECharacterShape shape)
    {
        CharacterShape = shape;
        OnShapeChanged?.Invoke(shape);
    }

    #endregion
}
