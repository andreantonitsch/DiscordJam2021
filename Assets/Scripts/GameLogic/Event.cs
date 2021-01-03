using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Event
{
    [System.Serializable]
    public enum EventType
    {
        CorruptionTick,
        SpawnTick,
        Damage,
        CorruptNode,
        UpdateDistanceFunction,
        NodesSpawned,
        SoldierSpawnTick,
        SoldierActTick,
        NodeDestroyed,
        NodeRecaptured,
        NodeAttackTick,
        UpdatePowerUps

    }

    public Event(EventType type)
    {
        Type = type;
    }

    public EventType Type;
    public string s_val;
    public float f_val1;
    public float f_val2;
    public int i_val1;
    public int i_val2;

    public override string ToString()
    {
        return $"{Type},{s_val},{f_val1},{f_val2},{i_val1},{i_val2}";
        return base.ToString();

    }
}
