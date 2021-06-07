using UnityEngine;
using System.Collections;

public class MathTool
{

    public static float piDivide180 = Mathf.PI / 180;

    public static bool IsFacingRight(Transform t)
    {
        if (t.localEulerAngles.y > 0) return false;
        else return true;
    }

    public static void FacingRight(Transform t)
    {
        t.localEulerAngles = new Vector3(0, 0, 0);
    }

    public static void FacingLeft(Transform t)
    {
        t.localEulerAngles = new Vector3(0, 180, 0);
    }

    public static Vector2 GetVector2(Vector3 a)
    {
        Vector2 posA = new Vector2(a.x, a.z);
        return posA;
    }

    public static float GetDistance(Transform a, Transform b)
    {
        Vector2 posA = GetVector2(a.position);
        Vector2 posB = GetVector2(b.position);
        return Vector2.Distance(posA, posB);
    }

    public static Vector2 GetDirection(Transform a, Transform b)
    {
        Vector2 posA = GetVector2(a.position);
        Vector2 posB = GetVector2(b.position);
        return posB - posA;
    }

}