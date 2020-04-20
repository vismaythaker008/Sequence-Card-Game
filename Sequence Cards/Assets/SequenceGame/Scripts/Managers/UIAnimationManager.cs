using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class UIAnimationManager : MonoBehaviour
{
    public List<UIAnimationCanvasComponent> uIAnimationComponents;
    public static UIAnimationManager instance;



    // public RectTransform[] S2UPANIMBTNYY2;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;


    }

    public void Animate(string ScreenName)
    {

        UIAnimationCanvasComponent temp = uIAnimationComponents.Find(x => x.ScreenName == ScreenName);


        //currentComponent = uIAnimationComponents[(int)animationMenu];
        foreach (UIAnimationRectComponent item in temp.uIAnimationComponent)
        {
            Debug.Log(item.ComponentName);
            if (item.direction == Directions.LeftRight)
            {
                Debug.Log("x");
                item.rect.DOAnchorPosX(item.endPoint, item.duration);
            }
            else
            {
                Debug.Log("y");
                item.rect.DOAnchorPosY(item.endPoint, item.duration);
            }


        }
        //Reset();

    }
    public void Reset(string ScreenName)
    {

        UIAnimationCanvasComponent temp = uIAnimationComponents.Find(x => x.ScreenName == ScreenName);
        //currentComponent = uIAnimationComponents[(int)animationMenu];
        foreach (UIAnimationRectComponent item in temp.uIAnimationComponent)
        {
            if (item.direction == Directions.LeftRight)
            {
                item.rect.DOMoveX(item.startPoint, item.duration);
            }
            else
            {
                item.rect.DOMoveY(item.startPoint, item.duration);
            }
        }
        //Reset();

    }


    // public void Reset()
    // {
    //     foreach (UIAnimationCanvasComponent item in uIAnimationComponents)
    //     {
    //         if (item.animationMenu != currentComponent.animationMenu)
    //             item.Reset(0.01f);
    //     }
    // }


}
[System.Serializable]
public class UIAnimationCanvasComponent
{

    public string ScreenName;
    public AnimationMenu animationMenu;
    public UIAnimationRectComponent[] uIAnimationComponent;

}

[System.Serializable]
public class UIAnimationRectComponent
{

    public string ComponentName;

    public Directions direction;
    public RectTransform rect;
    public float startPoint;
    public float endPoint;
    public float duration = 1;



}
public enum Directions // your custom enumeration
{
    UpDown,
    LeftRight
};
public enum AnimationMenu // your custom enumeration
{
    MainMenu,
    SelectionMenu,
    SettingsMenu,
    VersesUI,
    GamePlay,
    PausedGamePlay,
    GameOver,
    VersesGameOver
};

