using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ScaleUpDownScript : MonoBehaviour
{
    float elapsedTime = 0;
    float waitTime = 0.8f;
    private TextMeshProUGUI text;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        text = transform.GetComponent<TextMeshProUGUI>();
        StartCoroutine(ScaleUpDown());
    }
    IEnumerator ScaleUpDown()
    {
        while (true)
        {
            while (elapsedTime < waitTime)
            {
                text.fontSize = Mathf.Lerp(110, 155, (elapsedTime / waitTime));
                elapsedTime += Time.deltaTime;

                // Yield here
                yield return null;
            }
            elapsedTime = 0;
            // Make sure we got there
            text.fontSize = 155;
            while (elapsedTime < waitTime)
            {
                text.fontSize = Mathf.Lerp(155, 110, (elapsedTime / waitTime));
                elapsedTime += Time.deltaTime;

                // Yield here
                yield return null;
            }
            elapsedTime = 0;
            text.fontSize = 110;
            yield return null;
        }
    }
}
