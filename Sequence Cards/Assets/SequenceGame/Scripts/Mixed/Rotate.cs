using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SequenceCardGame;

public class Rotate : MonoBehaviour
{
    private Vector3 rotation;
    public bool selected = false;
    private Coroutine lastRoutine = null;



    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        rotation = transform.eulerAngles;
    }
    public void OnSelected()
    {
        if (!selected)
        {
            lastRoutine = StartCoroutine(rotate());
            selected = true;
        }
    }
    public void OnDeSelected()
    {
        StopCoroutine(lastRoutine);
        selected = false;
    }
    IEnumerator rotate()
    {
        while (true)
        {

            rotation.y += 10;
            // transform.Rotate(Vector3.left * 10, Space.Self);
            transform.eulerAngles = rotation;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
