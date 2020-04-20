using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private Coroutine spawn;
    private Coroutine CurvyRoad;
    void Start()
    {
        spawn = StartCoroutine(SpawnSphere());
        CurvyRoad = StartCoroutine(CurveRoad());
    }
    IEnumerator SpawnSphere()
    {
        yield return null;
    }
    IEnumerator CurveRoad()
    {
        yield return null;
    }


}
