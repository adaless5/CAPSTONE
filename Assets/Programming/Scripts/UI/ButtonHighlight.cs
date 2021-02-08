using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool IsHighlighted { get; private set; } = false;
     // When highlighted with mouse.
     public void OnPointerEnter(PointerEventData eventData)
    {
        IsHighlighted = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsHighlighted = false;
    }
}
