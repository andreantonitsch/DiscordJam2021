using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScaledTime
{

    public static float TimeScale = 1.0f;

    public static float deltaTime
    {
        get
        {
            return Time.deltaTime * TimeScale;
        }
    }

    public static float fixedDeltaTime
    {
        get
        {
            return Time.fixedDeltaTime * TimeScale;
        }
    }
}
