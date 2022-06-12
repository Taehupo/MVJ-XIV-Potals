using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShapesHolder : MonoBehaviour
{
    private List<Image> shapeIcons = new List<Image>();

    // Start is called before the first frame update
    void Start()
    {
        CharacterManager.Instance.ShapeController.OnShapeChange += UpdateIconList;
        UpdateIconList();
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void AddIcon()
    {
        GameObject icon = new GameObject("ShapeIcon", typeof(Image));
        icon.transform.SetParent(transform, false);
        //icon.transform.parent = transform;
        icon.transform.localPosition = Vector3.zero;
        shapeIcons.Add(icon.GetComponent<Image>());
    }

    private void RemoveIcon()
    {
        Image icon = shapeIcons[shapeIcons.Count];
        shapeIcons.RemoveAt(shapeIcons.Count);
        Destroy(icon);
    }

    private void UpdateIconList()
    {
        int numberOfIcons = GameManager.instance.usableShapes.Count;
        if (numberOfIcons > 1)
        {
            while (shapeIcons.Count < numberOfIcons)
                AddIcon();
            while (shapeIcons.Count > numberOfIcons)
                RemoveIcon();
            UpdateCurrentIcons();
        }
    }

    private void UpdateCurrentIcons()
    {
        for(int i = 0; i < shapeIcons.Count; i++)
        {
            Image image = shapeIcons[i];
            image.sprite = CharacterManager.Instance.SpriteManager.GetShapeVisuals(GameManager.instance.usableShapes[i]).UIIcon;
            if (CharacterManager.Instance.ShapeController.CharacterShape.Equals(GameManager.instance.usableShapes[i]))
            {
                image.color = new Color(1, 1, 1, 1);
            }
            else
            {
                image.color = new Color(1, 1, 1, 0.5f);
            }
        }
    }
}
