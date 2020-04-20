using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class Utilities
{
    public static IEnumerator Wait(float seconds, Action actiontoperform)
    {
        yield return new WaitForSeconds(seconds);

        if (actiontoperform != null)
        {
            actiontoperform();
        }

    }
    public static T KeyByValue<T, W>(this Dictionary<T, W> dict, W val)
    {
        T key = default;
        foreach (KeyValuePair<T, W> pair in dict)
        {
            if (EqualityComparer<W>.Default.Equals(pair.Value, val))
            {
                key = pair.Key;
                break;
            }
        }
        return key;
    }
    public static async void WaitAsync(int delayinms, Action actiontoperform)
    {
        await Task.Delay(delayinms);

        if (actiontoperform != null)
        {
            actiontoperform();
        }
    }
}
