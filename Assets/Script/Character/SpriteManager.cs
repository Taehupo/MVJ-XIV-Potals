using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    #region Members

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public static Action<ECharacterShape> OnVisualChanged;

    private readonly Dictionary<ECharacterShape, CharacterShapeVisuals> m_ShapeToVisualController = new();

    #endregion

    #region Public Manipulators

    public void ChangeShape(ECharacterShape shape)
    {
        SetVisuals(shape);
    }

    #endregion

    #region Inherited Manipulators

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        CreateShapeToVisual();

        // set default shape as Human
        SetVisuals(ECharacterShape.Human);
    }


    private void OnDestroy()
    {
        // clear dictionnary
        m_ShapeToVisualController.Clear();
    }

    #endregion

    #region Private Manipulators

    void CreateShapeToVisual()
    {
        foreach (CharacterShapeVisuals shapeVisuals in CharacterManager.Instance.ShapesVisuals)
        {
            if (m_ShapeToVisualController.ContainsKey(shapeVisuals.Shape))
            {
                Debug.LogError("SpriteManager.CreateShapeToVisual() Error : Duplicate CharacterShapeVisual in CharacterManager " + shapeVisuals.Shape);
                continue;
            }

            m_ShapeToVisualController.Add(shapeVisuals.Shape, shapeVisuals);
        }
    }

    private void SetVisuals(ECharacterShape shape)
    {
        if (shape == ECharacterShape.count)
        {
            Debug.LogError("ShapeController.SetShape() Error : Cannot set shape to " + shape);
            return;
        }
        SwapVisuals(shape);
        OnVisualChanged?.Invoke(shape);

    }

    void SwapVisuals(ECharacterShape shape)
    {
        if (m_ShapeToVisualController[shape] == null)
            Debug.LogError("SpriteManager.SwapVisuals() Error : No Visuals found for " + shape);
        else
        {
            // Sprite
            spriteRenderer.sprite = m_ShapeToVisualController[shape].ShapeSprite;

            // Animator
            animator.runtimeAnimatorController = m_ShapeToVisualController[shape].AnimatorController;
        }
    }

    #endregion
}
