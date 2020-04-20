using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SequenceCardGame;
using DG.Tweening;
public class UIManager : MonoBehaviour
{

    public static UIManager Instance;
    public List<ScreenData> ScreenDataList;
    public SceneName DefaultScreen;
    ScreenView currentScreen;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        foreach (var item in ScreenDataList)
        {
            item.screen.Disable();
        }

        ChangeScreen(DefaultScreen);


    }

    public void ChangeScreen(SceneName sceneName)
    {
        ScreenData temp = ScreenDataList.Find(x => x.screenName == sceneName);
        if (currentScreen != null)
        {
            currentScreen.Hide();
        }
        currentScreen = temp.screen;
        Debug.Log(currentScreen);
        currentScreen.Show();
        // currentScreen.GetComponent<RectTransform>().DOAnchorPos(Vector3.zero, 1);
    }


}

[System.Serializable]
public class ScreenData
{

    public SceneName screenName;
    public ScreenView screen;

}
