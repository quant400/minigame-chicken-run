using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIVirtualButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{

    [Header("Output")]
    public UnityEvent<bool> buttonStateOutputEvent;
    public UnityEvent buttonClickOutputEvent;
    public bool toggle;
    public bool currentToggleValue;
    [SerializeField]
    Sprite toggledImg;
    [SerializeField]
    Sprite nonToggleImg;

    public void OnPointerDown(PointerEventData eventData)
    {
        if(toggle)
        {
            currentToggleValue = !currentToggleValue;
            GetComponent<Button>().interactable = !currentToggleValue;
            OutputButtonStateValue(currentToggleValue);
            if(toggledImg!=null)
            {
                if (currentToggleValue)
                    GetComponent<Image>().sprite = toggledImg;
                else
                    GetComponent<Image>().sprite = nonToggleImg;
            }
        }
        else
        {
            OutputButtonStateValue(true);
        }
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(!toggle)
            OutputButtonStateValue(false);
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        OutputButtonClickEvent();
    }

    void OutputButtonStateValue(bool buttonState)
    {
        buttonStateOutputEvent.Invoke(buttonState);
    }

    void OutputButtonClickEvent()
    {
        buttonClickOutputEvent.Invoke();
    }

}
