using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public class PowerUpTooltipHandler : MonoBehaviour
     //, IPointerClickHandler // 2
     , IPointerEnterHandler
     , IPointerExitHandler
{
    //public Canvas canvas;
    public EventHandler eh;

    public GameObject Tooltip;


    public void OnPointerEnter(PointerEventData eventData)
    {
        Tooltip.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.SetActive(false);
    }
  

    //public TooltipScreenSpaceUI tooltip;


    public void Start()
    {
        eh = EventHandler.Instance;
    }


}
