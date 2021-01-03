using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : EventListener
{
    public static List<Soldier> ActiveSoldiers = new List<Soldier>();
    public GameObject bullet;

    public NodeController nc;
    public BaseParameters bp;
    public EventHandler eh;

    public float BaseFireRate;
    public float BaseDamage;
    public float BaseHP;

    public float FireRate;
    public float Damage;
    public float HP;

    public float FireTimer;

    public Transform Target; 
    public Node TargetNode; 

    public void Init(NodeStats node_stats)
    {
        nc = NodeController.Instance;
        bp = BaseParameters.Instance;
        eh = EventHandler.Instance;

        float total = nc.Nodes.Count;
        float free_cities = total - nc.CorruptNodes.Count;

        float scale = 1 - (free_cities / total );
        float strength = node_stats.SpawnStrength * bp.SoldierBaseScaling * scale ;

        FireRate = BaseFireRate;
        Damage = BaseDamage * strength;
        HP = BaseHP * strength;

        ActiveSoldiers.Add(this);
        eh.Sub(Event.EventType.SoldierActTick, this);

        SetTarget();
    }

    public void OnDisable()
    {
        eh.Unsub(Event.EventType.SoldierActTick, this);
        ActiveSoldiers.Remove(this);
    }


    public void Shoot()
    {
        var e = new Event(Event.EventType.Damage);
        e.f_val1 = Damage;
        TargetNode.Consume(e);

        var b = ObjectPool.Spawn(bullet, transform.position, Quaternion.identity).GetComponent<BulletMover>();
        b.target = Target;
    }

    public void SetTarget()
    {
        var ns = nc.CorruptNodes;
        float dist = 100f;
        foreach(var n in ns)
        {
            var n_dist = Vector3.Distance(n.transform.position, transform.position);
            if(n_dist < dist)
            {
                dist = n_dist;
                Target = n.transform;
                TargetNode = n;
            }
        }
    }

    public void Move(float dist = 0.0f)
    {
        if (dist < bp.SoldierShootRadius)
            return;

        var dir = (Target.position - transform.position).normalized;
        transform.Translate(dir * bp.SoldierMoveSpeed);

    }


    public void Act()
    {
        if (Target == null)
            SetTarget();

        if (Target == null)
            return;

        FireTimer += ScaledTime.deltaTime;
        float target_dist = Vector2.Distance(Target.transform.position, transform.position);
        if (FireRate < FireTimer && bp.SoldierShootRadius > target_dist)
        {
            Shoot();
        }
        Move(target_dist);
    }

    public void TakeDamage(float quantity)
    {
        HP -= quantity;
        if (HP < 0)
            ObjectPool.Despawn(this.gameObject);
    }

    public override void Consume(Event e)
    {
        switch(e.Type)
        {
            case Event.EventType.SoldierActTick:
                Act();
                break;
            case Event.EventType.Damage:
                TakeDamage(e.f_val1);
                break;
            default:
                break;
        }

    }

}
