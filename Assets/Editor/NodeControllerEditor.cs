using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(NodeController))]
public class NodeControllerEditor : Editor
{

    
    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();

        NodeController script = (NodeController)target;

        if (GUILayout.Button("Spawn Nodes"))
        {
            script.SpawnNodes();
        }

    }



}
#endif