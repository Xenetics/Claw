using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spotlight : MonoBehaviour
{
    public float colorChangeDuration = 1f;
    private float elapsed;
    private Light spotlight;
    public bool changeColors = false;
    private bool changeToColor = false;
    private Color from;
    private Color to;

    private void Awake()
    {
        spotlight = GetComponent<Light>();
    }

    private void Update()
    {
        if (changeColors)
        {
            LerpLightColor();
            if (elapsed > colorChangeDuration)
            {
                elapsed = 0f;
                SetLerpColors();
            }
        }
    }

    public void ChangeColors()
    {
        changeColors = true;
    }

    public void LerpToColor(Color color)
    {
        from = spotlight.color;
        to = color;
        changeColors = true;
    }

    private void SetLerpColors()
    {
        from = spotlight.color;
        to = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

    private void LerpLightColor()
    {
        elapsed += Time.deltaTime;
        spotlight.color = Color.Lerp(from, to, elapsed / colorChangeDuration);
    }
}
