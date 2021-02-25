using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RawImageFader : MonoBehaviour
{

    public float StartAlpha = 0;
    public float EndAlpha = 0;

    public float TransitionTime = 1;
    public bool AutoStartAlpha = false;

    float time = 0;
    RawImage image;
//    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        image = GetComponent<RawImage>();
        //    spriteRenderer = GetComponent<SpriteRenderer>();
        if (AutoStartAlpha)
            StartAlpha = image.color.a;
    }

    void Start()
    {
        time = 0;
    }

    void Update()
    {
        time += Time.deltaTime;

        if (time > TransitionTime)
        {
            enabled = false;
            return;
        }
        float ratio = time / TransitionTime;
        float d = EndAlpha - StartAlpha;
        Color newColor = image.color;
        newColor.a = StartAlpha + d * ratio;
        image.color = newColor;

    }
}
