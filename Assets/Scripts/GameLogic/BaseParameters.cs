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

    public float CorruptionResistanceScaling = 100.0f;
    public float TickTime = 1.0f;

    public Vector2Int NodeSlotsRange = new Vector2Int(1, 3);
    public Vector2Int HPRange = new Vector2Int(100, 300);

    public int MaxCorruptionMult = 3;
    public Vector2Int CorruptionHPRange = new Vector2Int(100, 300);
    public Vector2Int CorruptingPowerRange = new Vector2Int(1, 3);
    public Vector2Int CorruptionResistanceRange = new Vector2Int(10, 30);
    

    public Vector2 AttackPowerRange = new Vector2(15, 30);
    public Vector2 SpawnStrengthRange = new Vector2(1, 3);


    public Vector2 SpawnRateRange = new Vector2(2, 50);

    public float NeighborDistance = 1.0f;


    public float CorruptionTick = 1.0f;

}

