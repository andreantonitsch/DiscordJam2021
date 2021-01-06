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
    public PowerUpUIHandler pu_handler;

    public float timer = 0.0f;

    //public float Delay = .3f;
    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("trigger");
        var n = eventData.pointerEnter.GetComponent<Node>();
        pu_handler.ApplyPowerUp(n);
    }


    // Start is called before the first frame update
    void Start()
    {
        ui_handler = FindObjectOfType<NodeUIHandler>();
        pu_handler = FindObjectOfType<PowerUpUIHandler>();
    }

}
