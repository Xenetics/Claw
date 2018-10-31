using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

public class BetterBtn : Button 
{
    public UnityAction onClick;
    public UnityAction onRelease;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (onClick != null)
        {
            onClick.Invoke();
        }
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if(onRelease != null)
        {
            onRelease.Invoke();
        }
        base.OnPointerUp(eventData);
    }
}
