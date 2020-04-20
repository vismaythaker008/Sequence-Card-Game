using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DirectionalLightBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Rotate());
    }
    IEnumerator Rotate()
    {
        while (true)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x,transform.eulerAngles.y +10 , transform.eulerAngles.z);
            yield return new WaitForSeconds(0.05f);
        }

    }


}
