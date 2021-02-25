using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ScaleToScreen : MonoBehaviour
{
    public bool ScaleWidth = true;
    public bool ScaleHeight = true; 

    static Vector2 ScreenSizeToWorldUnits()
    {
        Vector2 size = Camera.main.ViewportToWorldPoint(new Vector2(1, 1)) - Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        return size;
    }

    void Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

        Vector2 currentSize = renderer.sprite.bounds.size;
        Vector2 screenSize = ScreenSizeToWorldUnits();

        if (ScaleWidth)
            currentSize.x = screenSize.x / currentSize.x;
        if (ScaleHeight)
            currentSize.y = screenSize.y / currentSize.y;
        transform.localScale = new Vector3(currentSize.x, currentSize.y, 1);
    }

}
