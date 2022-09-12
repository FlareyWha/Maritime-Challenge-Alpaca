using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonMouseSelector : MonoBehaviour, IPointerEnterHandler, IDeselectHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!EventSystem.current.alreadySelecting)
            EventSystem.current.SetSelectedGameObject(this.gameObject);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        this.GetComponent<Selectable>().OnPointerExit(null);
    }
}
