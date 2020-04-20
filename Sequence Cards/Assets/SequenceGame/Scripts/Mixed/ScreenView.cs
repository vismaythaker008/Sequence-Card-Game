using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenView : MonoBehaviour
{
    Canvas canvas;
    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }
    public virtual void Show()
    {
        // Debug.Log(canvas.name + " animate");
        //UIAnimationManager.instance.Animate(canvas.name);
        // Debug.Log(canvas.name + " animated");
        canvas.enabled = true;
    }
    public virtual void Hide()
    {
        Debug.Log(canvas.name + " reset");
        //UIAnimationManager.instance.Reset(canvas.name);
        Debug.Log(canvas.name + " reset done");
        canvas.enabled = false;
    }
    public virtual void Disable()
    {
        canvas.enabled = false;
    }

}
