using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SequenceCardGame;
using System;
using System.Threading.Tasks;

public class PlayerSelectionMenuUI : ScreenView
{
    public GameObject[] Chips;

    private void OnEnable()
    {
        GameStateManager.onGameStateChange += OnGameStateChange;

    }
    private void OnDisable()
    {
        GameStateManager.onGameStateChange += OnGameStateChange;
    }
    public override void Show()
    {
        base.Show();
        showChipsMenu();
    }
    public override void Hide()
    {
        base.Hide();
        hideChipsMenu();
    }
    public void showChipsMenu()
    {
        foreach (GameObject chip in Chips)
        {
            chip.SetActive(true);
        }
    }
    public void hideChipsMenu()
    {
        foreach (GameObject chip in Chips)
        {
            chip.SetActive(false);
        }
    }

    /* public void OnPracticeBtnClk() {

         UIManager.Instance.ChangeScreen(SceneName.GamePlay);
         GameStateManager.Instance.ChangeGameState(GameState.GamePlay);

     }*/

    public void OnBackBtnClk()
    {
        UIManager.Instance.ChangeScreen(SceneName.SelectionMenu);
        // UIAnimationManager.instance.Animate(AnimationMenu.GamePlay);
        GameStateManager.Instance.ChangeGameState(GameState.MainMenu);
    }
    public void OnPlayButtonClick()
    {
        UIManager.Instance.ChangeScreen(SceneName.GamePlay);
        // UIAnimationManager.instance.Animate(AnimationMenu.GamePlay);

        WaitAsync(1000, () =>
                    {
                        GameStateManager.Instance.ChangeGameState(GameState.GamePlay);
                        Debug.Log("gamestate change called");
                    });
    }
    async void WaitAsync(int delayinms, Action actiontoperform)
    {
        await Task.Delay(delayinms);

        if (actiontoperform != null)
        {
            actiontoperform();
        }
    }

    void OnGameStateChange(GameState currentGameState)
    {

    }

}
