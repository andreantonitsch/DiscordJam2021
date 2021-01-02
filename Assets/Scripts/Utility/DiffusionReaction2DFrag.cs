/// Based on https://www.karlsims.com/rd.html and https://groups.csail.mit.edu/mac/projects/amorphous/GrayScott/
/// Implements the Gray-Scott mode for Reaction and Diffusion 


using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DiffusionReaction2DFrag : MonoBehaviour
{
    [System.Serializable]
    public enum SimulationModes
    {
        Coral,
        Mitosis,
        Mitosis2,
        WormLike,
        TemporalChaos,
        MixedPhase,
        Bubbles,
        Waves,
        Holes,
        Chaos,
        Custom

    }

    private Vector2[] ParameterDoubles = new Vector2[] { new Vector2(0.055f, 0.062f),   //coral
                                                         new Vector2(0.03f, 0.062f),    //mitosis
                                                         new Vector2(0.025f, 0.06f),    //mitosis2
                                                         new Vector2(0.0416f, 0.0625f),   // worm
                                                         new Vector2(0.0175f, 0.0504f),   // Temporal Chaos
                                                         new Vector2(0.0404f, 0.0638f),   //mixed phase
                                                         new Vector2(0.099f, 0.058f),// bubbles
                                                         new Vector2(0.01f, 0.04f), //waves
                                                         new Vector2(0.03f, 0.055f),  //holes
                                                         new Vector2(0.03f, 0.054f) }; //chaos


    public SimulationModes SelectedMode;

    public CameraDraw cam_script;
    private Material visualization_mat;
    public Material SimulationShader;
    public Material DrawShader;
    public Material ClearShader;

    private int kernel;

    private RenderTexture _qBuffer0;
    private RenderTexture _qBuffer1;


    public Vector2 DiffusionRatio;
    public float f;
    public float k;


    public float _DELTATIME = 0.2f;

    public int _ROWS = 128;
    public int _COLS = 128;

    public Vector3 SeedValues;
    public Vector3 BGValues;

    public float radius = 4000.0f;
    public bool random = false;
    public bool empty_canvas = false;

    public int IterationsPerFrame = 15;

    public void SetF(string s)
    {
        try
        {
            f = float.Parse(s);
        }
        catch
        {

        }
    }

    public void SetK(string s)
    {
        try
        {

            k = float.Parse(s);
        }
        catch
        {

        }
    }

    public void SetSimulationMode(int value)
    {       
        SelectedMode = (SimulationModes)value;
    }

    private RenderTexture InitializeTexture()
    {
        var tex = new RenderTexture(_COLS, _ROWS, 0, RenderTextureFormat.ARGBFloat);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.Create();

        return tex;
    }

    private void AreaInitializer(bool random = false, bool clear = false)
    {
        float[] data = new float[_ROWS * _COLS * 4];
        int i = 0;

        Vector2 point = new Vector2(_ROWS / 2, _COLS / 2);

        for (int x = 0; x < _ROWS; x++)
        {
            for (int y = 0; y < _COLS; y++)
            {
                if (clear)
                {
                    data[i] = BGValues.x;
                    data[i + 1] = BGValues.y;
                    data[i + 2] = 0;
                    data[i + 3] = 0;
                    continue;
                }

                float dist = (point - new Vector2(x, y)).sqrMagnitude;
                if (dist < radius)
                {
                    if (random)
                    {
                        data[i] = Random.Range(0.0f, 1.0f);
                        data[i + 1] = Random.Range(0.0f, 1.0f);
                        data[i + 2] = 0;
                        data[i + 3] = 0;
                    }
                    else
                    {
                        data[i] = SeedValues.x;
                        data[i + 1] = SeedValues.y;
                        data[i + 2] = 0;
                        data[i + 3] = 0;
                    }
                }
                else
                {
                    if (random)
                    {
                        data[i] = Random.Range(0.0f, 1.0f);
                        data[i + 1] = Random.Range(0.0f, 1.0f);
                        data[i + 2] = 0;
                        data[i + 3] = 0;
                    }
                    else
                    {
                        data[i] = BGValues.x;
                        data[i + 1] = BGValues.y;
                        data[i + 2] = 0;
                        data[i + 3] = 0;
                    }
                }

                i +=4;
            }
        }
        Texture2D tex = new Texture2D(_COLS, _ROWS, TextureFormat.RGBAFloat, false, false);
        tex.SetPixelData<float>(data, 0);
        tex.Apply();

        Graphics.CopyTexture(tex, _qBuffer0);
        Destroy(tex);
    }


    void OnDestroy()
    {
        Destroy(_qBuffer0);
        Destroy(_qBuffer1);
    }

    private void SetProperties()
    {

        if (SelectedMode == SimulationModes.Custom)
        {
            SimulationShader.SetFloat("_F", f);
            SimulationShader.SetFloat("_K", k);
        }
        else
        {
            Vector2 mode = ParameterDoubles[(int)SelectedMode];
            SimulationShader.SetFloat("_F", mode.x);
            SimulationShader.SetFloat("_K", mode.y);
        }

        SimulationShader.SetVector("_DiffusionRatio", DiffusionRatio);
        SimulationShader.SetFloat("_DELTATIME", _DELTATIME);
        SimulationShader.SetFloat("_ROWS", _ROWS);
        SimulationShader.SetFloat("_COLS", _COLS);
    }

    private void SwapBuffers()
    {
        var aux = _qBuffer0;
        _qBuffer0 = _qBuffer1;
        _qBuffer1 = aux;
    }

    public void ClearTexture()
    {
        DrawShader.SetColor("_ClearColor", new Color(BGValues.x, BGValues.y, 0, 0));
        Graphics.Blit(_qBuffer0, _qBuffer1, ClearShader);

        SwapBuffers();
    }

    // Start is called before the first frame update
    void Start()
    {

        visualization_mat = cam_script.renderMaterial;

        SetProperties();

        _qBuffer0 = InitializeTexture();
        _qBuffer1 = InitializeTexture();

        if(!empty_canvas)
            AreaInitializer(random);

        visualization_mat.SetTexture("_Q", _qBuffer0);
    }
    public void MouseDraw()
    {

            DrawShader.SetFloat("_ROWS", _ROWS);
            DrawShader.SetFloat("_COLS", _COLS);
            Graphics.Blit(_qBuffer0, _qBuffer1, DrawShader);

            SwapBuffers();
        
    }
    private void Step()
    {
        for (int i = 0; i < IterationsPerFrame; i++)
        {
            SetProperties();
            Graphics.Blit(_qBuffer0, _qBuffer1, SimulationShader);

            SwapBuffers();
        }

        visualization_mat.SetTexture("_Q", _qBuffer0);
    }

    // Update is called once per frame
    void Update()
    {
        Step();
    }

}
