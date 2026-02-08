using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Unity.VisualScripting;

public class OnButton : MonoBehaviour, IPointerEnterHandler , IPointerExitHandler
{
    public TextMeshProUGUI UnLockedText;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        UnLockedText.gameObject.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData) 
    { 
        UnLockedText.gameObject.SetActive(false);
    }
    
}
