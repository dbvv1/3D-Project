using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethod 
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="transform">����Transform</param>
    /// <param name="targetTransform">Ŀ��Transform</param>
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
    /// <param name="transform">����Transition</param>
    /// <param name="targetTransform">Ŀ��Transition</param>
    /// <param name="facingAngle">Ҫ��ĽǶ�</param>
    /// <returns></returns>
    public static bool IsFacingTarget(this Transform transform,Transform targetTransform,float facingAngle)
    {
        if (Vector3.Dot(transform.forward, (targetTransform.position - transform.position).normalized) > Mathf.Cos(facingAngle)) 
            return true;
        return false;
    }

}
