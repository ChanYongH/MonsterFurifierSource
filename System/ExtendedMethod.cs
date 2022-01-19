using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtendedMethod
{
    public static bool Between(this float value, float min, float max)
    {
        if (min <= value && value <= max)
            return true;
        else
            return false;
    }
    public static bool Between(this int value, int min, int max) // 오버로딩
    {
        if (min <= value && value <= max)
            return true;
        else
            return false;
    }

    public static IEnumerator ObjectSwitch(this GameObject obj, float time)
    {
        obj.SetActive(true);
        yield return new WaitForSecondsRealtime(time);
        obj.SetActive(false);
    }

}
