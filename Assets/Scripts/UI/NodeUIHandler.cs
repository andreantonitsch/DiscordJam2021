using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NodeUIHandler : MonoBehaviour
     , IPointerClickHandler // 2
     , IPointerEnterHandler
     , IPointerExitHandler
{
    //public Canvas canvas;
    //public GameObject tooltipPrefab;
    //public TooltipScreenSpaceUI tooltip;

    public void Start()
    {
        //tooltip = Instantiate(tooltipPrefab).GetComponent<TooltipScreenSpaceUI>();
        //canvas = FindObjectOfType<Canvas>();
        //tooltip.rectTransform.parent = canvas.transform;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        //tooltip.Locked = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //tooltip.gameObject.SetActive(true);
        //tooltip.Locked = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse Exit");
        //tooltip.Locked = false;
        //tooltip.gameObject.SetActive(false);
    }
}
