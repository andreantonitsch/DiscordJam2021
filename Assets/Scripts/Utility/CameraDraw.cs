using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDraw : MonoBehaviour
{
    public Material renderMaterial;
    public string ImageName;
    public Vector2Int res;

    private Camera camera;


    private void Start()
    {
        camera = Camera.main;
    }

    protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, renderMaterial);
    }


}

