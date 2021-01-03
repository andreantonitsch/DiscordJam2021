using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(FungusRenderer))]
public class FungusRendererEditor : Editor
{

    
    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();

        FungusRenderer script = (FungusRenderer)target;

        if (GUILayout.Button("Update Distance Function"))
        {
            script.GetDistanceFunction();
            script.debug_material.SetTexture("_MainTex", script.distance_tex);
        }
        if (GUILayout.Button("Try Draw"))
        {
            script.diffusion_handler.DrawCenter();
            //script.debug_material.SetTexture("_MainTex", script.distance_tex);
        }

    }


}
#endif