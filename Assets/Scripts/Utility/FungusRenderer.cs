using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungusRenderer : EventListener
{

    #region Debug
    public Material debug_material;
    public bool DEBUG = false;
    #endregion

    public BaseParameters bp;
    public NodeController nc;
    public EventHandler eh;

    public DiffusionReaction2DFrag diffusion_handler;
    public RenderTexture distance_tex;

    public Material distance_function_mat;

    public Renderer BackgroundRenderer;

    public int MaxEstimatedNodes = 120;

    public bool dirty_tree = true;

    public void Start()
    {
        diffusion_handler = Component.FindObjectOfType<DiffusionReaction2DFrag>() as DiffusionReaction2DFrag;
        bp = BaseParameters.Instance;
        nc = NodeController.Instance;
        eh = EventHandler.Instance;

        eh.Sub(Event.EventType.CorruptNode, this);
        eh.Sub(Event.EventType.UpdateDistanceFunction, this);
        eh.Sub(Event.EventType.NodesSpawned, this);
    }

    public RenderTexture InitializeTexture()
    {
        var tex = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.Create();

        return tex;
    }

    public void GetDistanceFunction()
    {
        if (dirty_tree)
            GenerateCorruptionDistanceFunction();
        else
            UpdateDistanceFunction();

        diffusion_handler.SimulationShader.SetTexture("_InfluenceTex", distance_tex);

        if (DEBUG)
            debug_material.SetTexture("_MainTex", distance_tex);

    }

    public void GenerateCorruptionDistanceFunction()
    {
        if(distance_tex == null)
            distance_tex = InitializeTexture();

        distance_function_mat.SetVector("_Domain", bp.Domain);

        var nc_list = nc.CorruptNodes;
        float[] pos = new float[MaxEstimatedNodes * 2]; 
        float[] data = new float[MaxEstimatedNodes];

        for (int i = 0; i < nc_list.Count; i++)
        {
            var n = nc_list[i];
            pos[i * 2] = n.transform.position.x;
            pos[i * 2 + 1] = n.transform.position.y;
            data[i] = n.Corruption / n.BaseStats.CorruptionHP;
        }


        distance_function_mat.SetInt("_ArrayLength", nc_list.Count);
        distance_function_mat.SetFloatArray("_NodePosition", pos);
        distance_function_mat.SetFloatArray("_NodeData", data);

        Graphics.Blit(distance_tex, distance_tex, distance_function_mat);

        dirty_tree = false;

    }


    public void UpdateDistanceFunction()
    {
        var nc_list = nc.CorruptNodes;
        float[] data = new float[nc_list.Count];

        for (int i = 0; i < data.Length; i++)
        {
            var n = nc_list[i];
            data[i] = n.Corruption / n.BaseStats.CorruptionHP;
        }

        distance_function_mat.SetInt("_ArrayLength", nc_list.Count);
        distance_function_mat.SetFloatArray("_NodeData", data);

        Graphics.Blit(distance_tex, distance_tex, distance_function_mat);
    }

    public void DisposeDistanceTex()
    {
        if(distance_tex != null)
            distance_tex.Release();

    }

    public void OnDestroy()
    {
        DisposeDistanceTex();
    }

    public void Update()
    {
        diffusion_handler.Step();
    }

    public override void Consume(Event e)
    {
        switch (e.Type)
        {
            case Event.EventType.CorruptNode:
                dirty_tree = true;
                GetDistanceFunction();
                break;
            case Event.EventType.UpdateDistanceFunction:
                GetDistanceFunction();
                break;
            case Event.EventType.NodesSpawned:
                GetDistanceFunction();
                diffusion_handler.DrawCenter();
                break;
            default:
                break;
        }

    }


}
