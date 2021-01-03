using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentAnimator : MonoBehaviour
{
    public Transform Target;
    public Vector2 Offset = new Vector2(0.1f, 0.1f);
    
    // Update is called once per frame
    void Update()
    {
        var pos = Target.transform.position;
        var v = new Vector3(pos.x + Offset.x * Mathf.Sin(Time.deltaTime),
                            pos.y + Offset.y * Mathf.Cos(Time.deltaTime),
                            pos.z);
    }
}
