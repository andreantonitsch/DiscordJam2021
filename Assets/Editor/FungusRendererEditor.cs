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

    }


}
#endif