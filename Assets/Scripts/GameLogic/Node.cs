using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : EventListener
{
    #region Debug
    public bool DrawNeighbors = true;
    public bool DrawChildren = true;
    #endregion


    public GameObject SoldierPrefab;
    public GameObject NodeBullletPrefab;
    public List<Node> Neighbors = new List<Node>();

    public Node Parent;
    public List<Node> Children;

    public int SoldierSpawnTimer;
    public float Corruption;

    public float Energy;

    public float Damage;
    public bool Free = true;

    public Soldier ShootTarget;

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


    public bool Channeling = false;

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
        Corruption = Mathf.Min(bp.MaxCorruptionMult * CurrentStats.CorruptionHP, Corruption);


        if(Corruption > CurrentStats.CorruptionHP && parent != null && Free)
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

        eh.Unsub(Event.EventType.SoldierSpawnTick, this);
        eh.Sub(Event.EventType.NodeAttackTick, this);
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

    public void PropagateUpdate(List<PowerUp> effective = null)
    {
        EffectivePowerUps.Clear();
        EffectivePowerUps.AddRange(OwnPowerUps);

        foreach(var child in Children)
        {
            child.PropagateUpdate(EffectivePowerUps);
        }

        UpdateStats();
    }


    public void UpdateStats()
    {
        CurrentStats = new NodeStats(BaseStats);

        foreach (var item in EffectivePowerUps)
        {
            if(item != null)
            {
                if (OwnPowerUps.Contains(item)){
                    item.Apply(this);
                    item.Apply(this);
                    item.Apply(this);
                }else
                {
                    item.Apply(this);
                }
                
            }
        }
    }


    public bool AttachPowerUp(PowerUp p)
    {
        if(PowerUpSlots > OwnPowerUps.Count)
        {
            OwnPowerUps.Add(p);
            return true;
        }
        return false;
    }

    public void SpawnSoldier()
    {
        Vector3 soldier_pos = transform.position + new Vector3(bp.SoldierSpawnOffset.x * (Random.value - 0.5f), bp.SoldierSpawnOffset.y * (Random.value - 0.5f), 0.0f);

        var s =  ObjectPool.Spawn(SoldierPrefab, soldier_pos, Quaternion.identity);
        //var s = Instantiate(SoldierPrefab);
        //s.transform.position = soldier_pos;
        s.GetComponent<Soldier>().Init(CurrentStats);
    }


    public void SoldierTick()
    {
        SoldierSpawnTimer++;
        if(SoldierSpawnTimer >= CurrentStats.SpawnRate)
        {
            SpawnSoldier();
            SoldierSpawnTimer = 0;
        }
    }

    public void TakeDamage(float Quantity)
    {

        var base_resist = bp.DamageResistanceScaling;
        Damage += Quantity * (base_resist / (base_resist + CurrentStats.DamageResistance));
        if (Damage > CurrentStats.HP)
        {
            NodeDestroyed();
        }

    }
    public void NodeDestroyed()
    {
        var e = new Event(Event.EventType.NodeDestroyed);
        e.i_val1 = ID;
        eh.Unsub(Event.EventType.CorruptionTick, this);
        eh.Unsub(Event.EventType.NodeAttackTick, this);

        eh.Push(e);
    }

    public void AcquireTarget()
    {
        if (!(ShootTarget == null) && !ShootTarget.gameObject.activeSelf)
        {
            ShootTarget = null;
            return;
        }

        var p = transform.position;
        var x = p.x;
        var y = p.y;
        var v = new Vector2(x, y);

        foreach (var item in Soldier.ActiveSoldiers)
        {
            var p2 = item.transform.position;
            var x2 = p2.x;
            var y2 = p2.y;
            var v2 = new Vector2(x2, y2);
            var d = Vector2.Distance(p, p2);
            if (Vector2.Distance(p, p2) < CurrentStats.AttackRange)
            {
                ShootTarget = item;
                break;
            }
        }
    }

    public void Shoot()
    {
        AcquireTarget();
        
        if((ShootTarget != null && ShootTarget.isActiveAndEnabled)){
            var e = new Event(Event.EventType.Damage);
            e.f_val1 = CurrentStats.AttackPower;
            ShootTarget.Consume(e);

            var b = ObjectPool.Spawn(NodeBullletPrefab, transform.position, Quaternion.identity).GetComponent<BulletMover>();
            b.target = ShootTarget.transform;

        }

    }

    
    
    public override void Consume(Event e)
    {
        switch (e.Type)
        {
            case Event.EventType.CorruptionTick:
                AbsorbCorruption();
                break;
            case Event.EventType.SoldierSpawnTick:
                SoldierTick();
                break;            
            case Event.EventType.Damage:
                TakeDamage(e.f_val1);
                break;            
            case Event.EventType.NodeAttackTick:
                if(!Free)
                    Shoot();
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
