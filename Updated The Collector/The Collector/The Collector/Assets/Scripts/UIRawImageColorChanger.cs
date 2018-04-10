using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIRawImageColorChanger : EventTrigger, IPointerEnterHandler, ISelectHandler, IDeselectHandler
{
    public RawImage imageToChange;

    public Color normalColor = new Color(245f,245f,245f,255f);
    public Color highlightColor = new Color(159f, 159f, 159f, 255f);
    public Color pressedColor = new Color(194f, 194f, 194f, 255f);

    public override void OnDeselect(BaseEventData data)
    {
        imageToChange.color = normalColor;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        imageToChange.color = highlightColor;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        imageToChange.color = highlightColor;
    }

    public override void OnSelect(BaseEventData eventData)
    {
        imageToChange.color = highlightColor;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        imageToChange.color = pressedColor;
    }
}
