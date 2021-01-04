using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public GameObject Intro1;
    public GameObject Intro2;
    public GameObject IntroPause;
    public GameObject CreditsTab;

    public GameObject Victory;
    public GameObject Loss;


    NodeController nc;
    BaseParameters bp;
    EventHandler eh;
    DiffusionReaction2DFrag diffusion_handler;
    public float TimeScale =  1.0f;

    public float CorruptionTimer = 0.0f;
    public float SoldierSpawnTimer = 0.0f;
    public float SoldierActTimer = 0.0f;
    public float NodeAttackTimer = 0.0f;
    public float UpdateDistTimer = 0.0f;
    public float PowerUpTimer = 0.0f;

    public enum State
    {
        Paused,
        Game
        
    }

    public State state = State.Game;

    public IEnumerator delayed_Draw()
    {
        yield return new WaitForSeconds(1);

        diffusion_handler.DrawCenter();
    }

    public void Start()
    {
        nc = NodeController.Instance;
        bp = BaseParameters.Instance;
        eh = EventHandler.Instance;
        diffusion_handler = FindObjectOfType<DiffusionReaction2DFrag>();
        diffusion_handler.SelectedMode = DiffusionReaction2DFrag.SimulationModes.WormLike;
        ScaledTime.TimeScale = 0.0f;
        StartCoroutine(delayed_Draw());
    }

    public void BeginSimulation()
    {
        ScaledTime.TimeScale = 1.0f;
        diffusion_handler.ClearTexture();
        diffusion_handler.DrawCenter();
        diffusion_handler.SelectedMode = DiffusionReaction2DFrag.SimulationModes.Coral;
        nc.SpawnNodes();
    }

    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }

    public void Mute()
    {
        var audio = FindObjectOfType<AudioSource>();
        audio.mute = !audio.mute;
    }

    public void Pause()
    {
        if (state == State.Game)
        {
            IntroPause.SetActive(true);
            ScaledTime.TimeScale = 0.0f;
            state = State.Paused;
        }
        else
        {
            IntroPause.SetActive(false);
            ScaledTime.TimeScale = 1.0f;
            state = State.Game;
        }
    }

    public void CloseStep1()
    {
        Intro1.SetActive(false);
        Intro2.SetActive(true);
    }
    public void CloseStep2()
    {
        Intro2.SetActive(false);
        BeginSimulation();
    }

    public void Credits()
    {
        CreditsTab.SetActive(!CreditsTab.activeSelf);
    }

    public void Lose()
    {
        ScaledTime.TimeScale = 0.0f;
        Loss.SetActive(true);
}

    public void Win()
    {
        ScaledTime.TimeScale = 0.0f;
        Victory.SetActive(true);
    }


    public void ChangeSpeed(float scale)
    {
        TimeScale = scale;
        ScaledTime.TimeScale = TimeScale;
    }

    public void GameTick()
    {
        CorruptionTimer += ScaledTime.fixedDeltaTime;
        SoldierSpawnTimer += ScaledTime.fixedDeltaTime;
        SoldierActTimer += ScaledTime.fixedDeltaTime;
        NodeAttackTimer += ScaledTime.fixedDeltaTime;
        UpdateDistTimer += ScaledTime.fixedDeltaTime;
        PowerUpTimer += ScaledTime.fixedDeltaTime;

        if (CorruptionTimer > bp.CorruptionTick)
        {
            var nodes = nc.Nodes;

            eh.Push(new Event(Event.EventType.CorruptionTick));

            CorruptionTimer = 0.0f;
        }

        if (SoldierSpawnTimer > bp.SoldierSpawnTick)
        {
            eh.Push(new Event(Event.EventType.SoldierSpawnTick));
            SoldierSpawnTimer = 0.0f;
        }

        if (SoldierActTimer > bp.SoldierActTick)
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
            if (nc.Nodes.Count > 0)
                eh.Push(new Event(Event.EventType.UpdateDistanceFunction));
            UpdateDistTimer = 0.0f;
        }

        if (PowerUpTimer > bp.PowerUpTick)
        {
            eh.Push(new Event(Event.EventType.PowerUpTick));
            PowerUpTimer = 0.0f;
        }
    }
        public void FixedUpdate()
        {
            GameTick();
        }

}
