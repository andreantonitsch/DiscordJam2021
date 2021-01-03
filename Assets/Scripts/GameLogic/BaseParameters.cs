using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseParameters : MonoBehaviour
{
    static BaseParameters instance;
    public static BaseParameters Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.Find("BaseParameters").GetComponent<BaseParameters>();
            }
            return instance;
        }
    }

    public Vector4 Domain;


    public float TickTime = 1.0f;

    public Vector2Int NodeSlotsRange = new Vector2Int(1, 3);
    public Vector2Int HPRange = new Vector2Int(100, 300);

    public int MaxCorruptionMult = 3;

    public Vector2Int CorruptionHPRange = new Vector2Int(100, 300);
    public Vector2Int CorruptingPowerRange = new Vector2Int(1, 3);
    public Vector2Int CorruptionResistanceRange = new Vector2Int(10, 30);

    public Vector2Int DamageResistanceRange = new Vector2Int(10, 30);
    public Vector2 AttackPowerRange = new Vector2(15, 30);
    public Vector2 AttackRateRange = new Vector2(3, 6);
    public Vector2 AttackRangeRange = new Vector2(1, 2);
    public Vector2 EnergyRange = new Vector2(100, 200);




    public Vector2 SpawnStrengthRange = new Vector2(1, 3);
    public Vector2 SpawnRateRange = new Vector2(15, 50);
    public Vector2 SoldierSpawnOffset = new Vector2(0.1f, 0.1f);
    public float SoldierMoveSpeed = 0.05f;
    public float SoldierShootRadius = 0.1f;



    public float CorruptionResistanceScaling = 100.0f;
    public float DamageResistanceScaling = 100.0f;
    public float EnergyScaling = 100;
    public float SoldierBaseScaling = 2.0f;

    public float NeighborDistance = 1.0f;


    public float CorruptionTick = 1.0f;
    public float SoldierSpawnTick = 1.0f;
    public float SoldierActTick = 0.1f;
    public float NodeAttackTick = 1.0f;

}

