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
        
        if(CorruptionTimer > bp.CorruptionTick)
        {
            var nodes = nc.Nodes;

            foreach (var n in nodes)
            {
                n.AbsorbCorruption();
            }
            CorruptionTimer = 0.0f;
        }



    }

    public void Update()
    {
        GameTick();
    }

}
