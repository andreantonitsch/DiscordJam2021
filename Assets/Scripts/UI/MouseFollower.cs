using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    public RectTransform rect;
    public Canvas canvas;

    public Vector3 Offset;

    public void Update()
    {
        rect.anchoredPosition = (Input.mousePosition / canvas.transform.localScale.x) + Offset;
    }
}
