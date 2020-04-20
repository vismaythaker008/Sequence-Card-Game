using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SequenceCardGame;
using UnityEngine.UI;

public class SettingsMenuUI : ScreenView
{
    public Button SoundON;
    public Button SoundOFF;
    public void OnEnable()
    {
        GameStateManager.onGameStateChange += OnGameStateChange;

    }
    public void OnDisable()
    {
        GameStateManager.onGameStateChange -= OnGameStateChange;

    }
    // public override void Show()
    // {
    //     base.Show();
    // }
    // public override void Hide()
    // {
    //     base.Hide();
    // }
    public void OnSoundTurnOnBtnClk()
    {
        SoundOFF.gameObject.SetActive(false);
        SoundON.gameObject.SetActive(true);
        AudioManager.instance.UnMute();
    }
    public void OnSoundTurnOffBtnClk()
    {
        SoundOFF.gameObject.SetActive(true);
        SoundON.gameObject.SetActive(false);
        AudioManager.instance.Mute();
    }
    public void OnChangeCharacterBtnClk()
    {

        // UIManager.Instance.ChangeScreen(SceneName.PlayerSelection);
        GameStateManager.Instance.ChangeGameState(GameState.MainMenu);
    }
    public void OnHomeBtnClk()
    {

        UIManager.Instance.ChangeScreen(SceneName.MainMenu);
        // UIAnimationManager.instance.Animate(AnimationMenu.MainMenu);
        GameStateManager.Instance.ChangeGameState(GameState.MainMenu);
    }
    public void OnChangeBallBtnClk()
    {

        // UIManager.Instance.ChangeScreen(SceneName.BallSelection);
        GameStateManager.Instance.ChangeGameState(GameState.MainMenu);
    }
    /*public void OnSelectionMenuBtnClk() {

        UIManager.Instance.ChangeScreen(SceneName.SelectionMenu);
        GameStateManager.Instance.ChangeGameState(GameState.SelectionMenu);
    }*/

    public void OnGameStateChange(GameState currentGameState)
    {


    }
}
