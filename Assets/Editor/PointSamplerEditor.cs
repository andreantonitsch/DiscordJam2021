using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(PointSampler))]
public class PointSamplerEditor : Editor
{

    
    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();

        PointSampler script = (PointSampler)target;

        if (GUILayout.Button("SamplePoints"))
        {

            var points = script.SamplePoints();
            var holder = GameObject.Find("NodeHolder").transform;

            for (int i = 0; i < points.Count; i++)
            {
                var obj = GameObject.Instantiate(script.DebugObject);

                var p = points[i];
                obj.transform.parent = holder;
                obj.transform.position = new Vector3(p.x, p.y, obj.transform.position.z);
            }
        }

    }



}
#endif