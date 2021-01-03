using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    static GameController instance;
    public static GameController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.Find("GameController").GetComponent<GameController>();
            }
            return instance;
        }
    }


    NodeController nc;
    BaseParameters bp;
    EventHandler eh;
    public float TimeScale =  1.0f;

    public float CorruptionTimer = 0.0f;
    public float SoldierSpawnTimer = 0.0f;
    public float SoldierActTimer = 0.0f;
    public float NodeAttackTimer = 0.0f;
    public float UpdateDistTimer = 0.0f;
    public float EnergyDrainTimer = 0.0f;



    public void Start()
    {
        nc = NodeController.Instance;
        bp = BaseParameters.Instance;
        eh = EventHandler.Instance;
    }

    public void ChangeSpeed(float scale)
    {
        TimeScale = scale;
        ScaledTime.TimeScale = TimeScale;
    }

    public void GameTick()
    {
        CorruptionTimer += ScaledTime.deltaTime;
        SoldierSpawnTimer += ScaledTime.deltaTime;
        SoldierActTimer += ScaledTime.deltaTime;
        NodeAttackTimer += ScaledTime.deltaTime;
        UpdateDistTimer += ScaledTime.deltaTime;
        //EnergyDrainTimer += ScaledTime.deltaTime;
        
        if(CorruptionTimer > bp.CorruptionTick)
        {
            var nodes = nc.Nodes;

            eh.Push(new Event(Event.EventType.CorruptionTick));

            CorruptionTimer = 0.0f;
        }

        if(SoldierSpawnTimer > bp.SoldierSpawnTick)
        {
            eh.Push(new Event(Event.EventType.SoldierSpawnTick));
            SoldierSpawnTimer = 0.0f;
        }
        
        if(SoldierActTimer > bp.SoldierActTick)
        {
            eh.Push(new Event(Event.EventType.SoldierActTick));
            SoldierActTimer = 0.0f;
        }

        if (NodeAttackTimer > bp.NodeAttackTick)
        {
            eh.Push(new Event(Event.EventType.NodeAttackTick));
            NodeAttackTimer = 0.0f;
        }
        if (UpdateDistTimer > bp.UpdateDistTick)
        {
            if(nc.Nodes.Count >0)
                eh.Push(new Event(Event.EventType.UpdateDistanceFunction));
            UpdateDistTimer = 0.0f;
        }


    }

    public void Update()
    {
        GameTick();
    }

}
