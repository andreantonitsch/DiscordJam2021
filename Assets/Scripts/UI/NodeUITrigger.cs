using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NodeUITrigger : MonoBehaviour
    ,IPointerClickHandler // 2
    //,IPointerEnterHandler
    //,IPointerExitHandler
{
    public NodeUIHandler ui_handler;

    public float timer = 0.0f;

    //public float Delay = .3f;
    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("trigger");
        var n = eventData.pointerEnter.GetComponent<Node>();
        ui_handler.InjectData(n);
    }

    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    ui_handler.Lock();
    //}  

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    timer = Delay;
    //    var n = eventData.pointerEnter.GetComponent<Node>();
    //    ui_handler.InjectData(n);
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    if (timer > 0.0f)
    //        return;
    //    ui_handler.MoveAway();
    //}

    // Start is called before the first frame update
    void Start()
    {
        ui_handler = FindObjectOfType<NodeUIHandler>();
    }

    //void Update()
    //{
    //    timer -= Time.deltaTime;
    //}
}
