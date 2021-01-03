using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public class NodeUIHandler : MonoBehaviour
     //, IPointerClickHandler // 2
     //, IPointerEnterHandler
     //, IPointerExitHandler
{
    //public Canvas canvas;
    public EventHandler eh;
    public MouseFollower follower;

    public Node displayed_node;

    Physics2DRaycaster raycaster;
    public GameObject NodeTooltip;
    public TextMeshProUGUI text_data;

    public GameObject[] Slots;

    public bool Locked;
    public float timer = 0.0f;
    public float LockTimer;

    //public TooltipScreenSpaceUI tooltip;


    public void Start()
    {
        eh = EventHandler.Instance;
    }
    public void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0.0f)
        {
            Unlock();
        }
    }

    public void Unlock()
    {
        Locked = false;
        follower.enabled = true;
    }


    public void Lock()
    {
        Locked = true;
        timer = LockTimer;
        follower.enabled = false;

    }


    public void MoveAway()
    { 
        if(!Locked)
            NodeTooltip.SetActive(false);
    }

    public void SetSlots(Node n)
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            if(i<n.PowerUpSlots)
                Slots[i].SetActive(true);
            else
                Slots[i].SetActive(false);
        }
    }

    public string Generate_Data_Text(Node n)
    {
        string s = $"{n.ID}\n{n.Corruption / n.CurrentStats.CorruptionHP}\n{n.CurrentStats.HP}\n{n.CurrentStats.AttackPower}";
        return s;
    }

    public void InjectData(Node n)
    {
        displayed_node = n;

        text_data.text = Generate_Data_Text(n);

        SetSlots(n);
        
        NodeTooltip.SetActive(true);

    }


    public void TriggerEnergyChannel()
    {
        var e = new Event(Event.EventType.NodeChannel);
        e.i_val1 = displayed_node.ID;
        eh.Push(e);
    }
    
}
