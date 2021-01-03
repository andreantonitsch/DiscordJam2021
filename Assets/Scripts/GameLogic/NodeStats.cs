using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodeStats
{
    public float CorruptingPower;
    public float CorruptionResistance;
    public float DamageResistance;
    public float CorruptionHP;
    public float HP;
    public float SpawnRate;
    public float SpawnStrength;
    public float AttackPower;
    public float AttackRate;
    public float AttackRange;

    public NodeStats() {    }

    public void Init()
    {
        var bp = BaseParameters.Instance;
        CorruptingPower = Random.Range(bp.CorruptingPowerRange.x, bp.CorruptingPowerRange.y);
        CorruptionResistance = Random.Range(bp.CorruptionResistanceRange.x, bp.CorruptionResistanceRange.y);
        DamageResistance = Random.Range(bp.DamageResistanceRange.x, bp.DamageResistanceRange.y);
        HP = Random.Range(bp.HPRange.x, bp.HPRange.y);
        SpawnRate = Random.Range(bp.SpawnRateRange.x, bp.SpawnRateRange.y);
        SpawnStrength = Random.Range(bp.SpawnStrengthRange.x, bp.SpawnStrengthRange.y);
        AttackPower = Random.Range(bp.AttackPowerRange.x, bp.AttackPowerRange.y);
        AttackRate = Random.Range(bp.AttackRateRange.x, bp.AttackRateRange.y);
        AttackRange = Random.Range(bp.AttackRangeRange.x, bp.AttackRangeRange.y);
        CorruptionHP = Random.Range(bp.CorruptionHPRange.x, bp.CorruptionHPRange.y);
    }

    public NodeStats(NodeStats copy)
    {

        CorruptingPower = copy.CorruptingPower;
        CorruptionResistance = copy.CorruptionResistance;
        DamageResistance = copy.DamageResistance;
        CorruptionHP = copy.CorruptionHP;
        HP = copy.HP;
        SpawnRate = copy.SpawnRate;
        SpawnStrength = copy.SpawnStrength;
        AttackPower = copy.AttackPower;
        AttackRate = copy.AttackRate;
        AttackRange = copy.AttackRange;

    }


}
