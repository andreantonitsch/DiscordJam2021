using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungusResolutionController : MonoBehaviour
{

    public FungusRenderer fungusRenderer;
    public DiffusionReaction2DFrag diffusion_handler;
    public Material DisplayMaterial;

    public void Start()
    {
        SetLowFungusRes();
    }

    public void UpdateMaterials(int r, int c)
    {
        diffusion_handler._ROWS = r;
        diffusion_handler._COLS = c;
        
        fungusRenderer.SetTextureResolution(r, c);
        DisplayMaterial.SetInt("_Rows", r);
        DisplayMaterial.SetInt("_Cols", c);
        diffusion_handler.InitializeTextures();
        diffusion_handler.DrawCenter();
    }

    public void SetLowFungusRes() 
    {
        diffusion_handler.IterationsPerFrame = 15;
        UpdateMaterials(256, 256);
    }
    public void SetMedFungusRes()
    {
        diffusion_handler.IterationsPerFrame = 25;
        UpdateMaterials(512, 512);
    }
    public void SetHighFungusRes() 
    { 
        diffusion_handler.IterationsPerFrame = 40;
        UpdateMaterials(1024, 1024);
    }

}
