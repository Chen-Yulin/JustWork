using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UICurve
{
    // Start is called before the first frame update
    public static float BounceCurve(float time)
    {
        if (time > 2f)
        {
            return 1;
        }
        return 0.35f * ((6f * time * time) / (10f * Mathf.Pow(time, 6) + 1) + 1.9f * Mathf.Atan(6f * time));
    }
}
