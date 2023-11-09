using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethod 
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="目标的transform"></param>
    /// <returns></returns>
    public static bool IsFacingTarget(this Transform transform, Transform targetTransform)
    {
        if (Vector3.Dot(transform.forward, (targetTransform.position - transform.position).normalized) > 0.35f)
            return true;
        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="目标的transform"></param>
    /// <param name="人物和目标之间角度的最小要求(0,90)"></param>
    /// <returns></returns>
    public static bool IsFacingTarget(this Transform transform,Transform targetTransform,float faceingAngle)
    {
        if (Vector3.Dot(transform.forward, (targetTransform.position - transform.position).normalized) > Mathf.Cos(faceingAngle)) 
            return true;
        return false;
    }

}
