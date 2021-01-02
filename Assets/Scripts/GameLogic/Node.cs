using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : EventListener
{
    #region Debug
    public bool DrawNeighbors = true;
    public bool DrawChildren = true;
    #endregion

    public List<Node> Neighbors = new List<Node>();

    public Node Parent;
    public List<Node> Children;


    public float Corruption;
    public bool Free = true;

    public int PowerUpSlots;
    public List<PowerUp> OwnPowerUps = new List<PowerUp>();
    public List<PowerUp> EffectivePowerUps = new List<PowerUp>();

    public NodeStats BaseStats;
    public NodeStats CurrentStats;

    public EventHandler eh;
    public BaseParameters bp;
    public NodeController nc;

    public int ID;

    public void Init()
    {

        bp = BaseParameters.Instance;
        nc = NodeController.Instance;
        eh = EventHandler.Instance;

        eh.Sub(Event.EventType.CorruptionTick, this);
    }


    public void AbsorbCorruption()
    {
        float corruption = 0;

        Node parent = null;
        float parent_corruption = 0.0f;

        foreach (Node n in Neighbors)
        {
            if (!n.Free && n.Corruption > parent_corruption)
            {
                parent = n;
                parent_corruption = n.Corruption;
            }
            if (!n.Free)
            {
                float base_resist = BaseParameters.Instance.CorruptionResistanceScaling;
                var current_n = n.CurrentStats;
                corruption += (n.Corruption / current_n.CorruptionHP) * current_n.CorruptingPower * (base_resist / (base_resist + CurrentStats.CorruptionResistance));
            }
        }

        Corruption += corruption;

        if(Corruption > BaseStats.CorruptionHP && parent != null && Free)
        {
            Free = false;
            CorruptionEvent(parent, this);
        }
    }

    public void CorruptionEvent(Node parent, Node child)
    {
        var corruption_event = new Event(Event.EventType.CorruptNode);

        corruption_event.i_val1 = parent.ID;
        corruption_event.i_val2 = child.ID;

        eh.Push(corruption_event);

    }

    public void FindNeighbors(float distance = 0.0f)
    {
        float dist;
        if (distance != 0.0f)
            dist = distance;
        else
            dist = bp.NeighborDistance;

        Neighbors.Clear();

        foreach(Node n in nc.Nodes)
        {
            if (Vector3.Distance(n.transform.position, this.transform.position) < dist && n != this)
            {
                Neighbors.Add(n);
            }
        }

    }

    public void PropagateUpdate()
    {

    }


    public void UpdateStats()
    {
        CurrentStats = new NodeStats(BaseStats);
    }


    public bool AttachPowerUp(PowerUp p)
    {
        return false;
    }

    public override void Consume(Event e)
    {
        switch (e.Type)
        {
            case Event.EventType.CorruptionTick:
                AbsorbCorruption();
                break;
            default:
                break;
        }

    }


    void OnDrawGizmosSelected()
    {
        if(DrawNeighbors)
            foreach(var n in Neighbors) 
            { 
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, n.transform.position);
            }
        if (DrawChildren)
            foreach (var n in Children)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, n.transform.position);
            }
    }


}
