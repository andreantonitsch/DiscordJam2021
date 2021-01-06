using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PowerUpUIHandler : EventListener
{
    public RectTransform selection_highlight;
    public GameObject[] PrototypePowerUps;

    public List<PowerUp> ActivePowerUps;
    public RectTransform ActivePowerUpHolder;

    public EventHandler eh;
    public PowerUp SelectedPowerUp;
    private int MaxPowerUPs = 10;
    
    public void Start()
    {
        eh = EventHandler.Instance;

        //n_ui = FindObjectOfType<NodeUIHandler>();

        eh.Sub(Event.EventType.PowerUpTick, this);
    }


    public void GetPowerUp()
    {
        if (ActivePowerUps.Count >= MaxPowerUPs)
            return;

        int i = Random.Range(0, 3);

        var chosen = Instantiate(PrototypePowerUps[i]).GetComponent<PowerUp>();
        chosen.gameObject.SetActive(true);

        chosen.GetComponent<RectTransform>().parent = ActivePowerUpHolder;
        ActivePowerUps.Add(chosen);
        AdjustList();
    }

    public void AdjustList()
    {
        int i = 0;
        foreach(var v in ActivePowerUps)
        {
            Vector2 p = new Vector2(0.0f, -15.0f - i * 40.0f);
            v.GetComponent<RectTransform>().anchoredPosition = p;
            i++;
        }
    }

    public void SelectPowerUp(BaseEventData eventData)
    {
        if (SelectedPowerUp != null)
            return;

        var selected_Power_up = eventData.selectedObject.GetComponent<PowerUp>();

        selection_highlight.gameObject.SetActive(true);
        selection_highlight.position = selected_Power_up.GetComponentInParent<RectTransform>().position - new Vector3(0, 13,0);

        ActivePowerUps.Remove(selected_Power_up);
        selected_Power_up.transform.parent = null;

        AdjustList();

        SelectedPowerUp = selected_Power_up;

    }

    //public void ApplyPowerUp(BaseEventData eventData)
    //{
    //    //Debug.Log("Apply Power Up");
    //    if (n_ui.displayed_node == null)
    //        return;

    //    var target_node = n_ui.displayed_node;
    //    if(target_node.PowerUpSlots - target_node.OwnPowerUps.Count  == 0)
    //        return;
    //    selection_highlight.gameObject.SetActive(false);

    //    var p_up = SelectedPowerUp;
    //    SelectedPowerUp = null;

    //    bool attached = target_node.AttachPowerUp(p_up);

    //    if (attached)
    //    {
    //        n_ui.InjectData(target_node);
    //        p_up.Attach_Attachments(target_node);
    //    }

    //    eh.Push(new Event(Event.EventType.UpdatePowerUps));
    //}

    public void ApplyPowerUp(Node n)
    {

        var target_node = n;
        if (target_node.PowerUpSlots - target_node.OwnPowerUps.Count == 0)
            return;
        selection_highlight.gameObject.SetActive(false);

        var p_up = SelectedPowerUp;
        SelectedPowerUp = null;

        bool attached = target_node.AttachPowerUp(p_up);

        if (attached)
        {
            p_up.Attach_Attachments(target_node);
        }

        eh.Push(new Event(Event.EventType.UpdatePowerUps));
    }

    public override void Consume(Event e)
    {
        base.Consume(e);
        switch (e.Type)
        {
            case Event.EventType.PowerUpTick:
                GetPowerUp();
                break;
            default:
                break;
        }

    }



}
