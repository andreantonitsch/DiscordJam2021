using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodeStats
{
    public float CorruptingPower;
    public float CorruptionResistance;
    public float CorruptionHP;
    public float HP;
    public float SpawnRate;
    public float SpawnStrength;
    public float AttackPower;

    public NodeStats() { }

    public NodeStats(NodeStats copy)
    {

        CorruptingPower = copy.CorruptingPower;
        CorruptionResistance = copy.CorruptionResistance;
        CorruptionHP = copy.CorruptionHP;
        HP = copy.HP;
        SpawnRate = copy.SpawnRate;
        SpawnStrength = copy.SpawnStrength;
        AttackPower = copy.AttackPower;

    }


}
