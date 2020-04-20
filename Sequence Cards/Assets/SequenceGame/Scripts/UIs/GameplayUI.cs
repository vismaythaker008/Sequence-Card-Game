using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SequenceCardGame;
using UnityEngine.UI;
using TMPro;
public class GameplayUI : ScreenView
{
    public GameObject[] PlayerNames;
    public Image[] TurnImages;
    public ModeManager modeManager;
    public ScoreManager scoreManager;
    private Coroutine Timer;
    int tempIndex = -1;


    private void OnEnable()
    {
        // scoreManager.ScoreChanged += OnScoreChanged;
        GameStateManager.onGameStateChange += OnGameStateChange;
        GameManagerScript.ShowTurnImage += showTurnImage;
        // modeManager.ModeChanged += OnGameModeChange;
    }
    private void OnDisable()
    {
        // scoreManager.ScoreChanged -= OnScoreChanged;
        GameStateManager.onGameStateChange -= OnGameStateChange;
        GameManagerScript.ShowTurnImage -= showTurnImage;
        // modeManager.ModeChanged -= OnGameModeChange;
    }
    public override void Show()
    {
        base.Show();
        Debug.Log("In Game Play");

        CardsManagerScript.instance.setPlayerCount(GameManagerScript.instance.PlayerCount);
        GameManagerScript.instance.ShowCards();
        GameManagerScript.instance.makePlayersReady();
        GameManagerScript.instance.startGame();
        showPlayerNames();

        //CardsManagerScript.instance.player.ShowCards();
    }

    public override void Hide()
    {
        base.Hide();
        GameManagerScript.instance.HideCards();
        // GameStateManager.onGameStateChange -= OnGameStateChange;
        CardsManagerScript.instance.player.HideCards();
        GameManagerScript.instance.endGame();
    }
    void showPlayerNames()
    {
        for (int i = 0; i < 3; i++)
        {
            if (i < GameManagerScript.instance.PlayerCount)
                PlayerNames[i].SetActive(true);
            else
                PlayerNames[i].SetActive(false);

        }
    }
    public void setPlayerNames(string player1, string player2, string player3)
    {

        PlayerNames[0].GetComponentInChildren<TextMeshProUGUI>().text = player1;
        PlayerNames[1].GetComponentInChildren<TextMeshProUGUI>().text = player2;
        PlayerNames[2].GetComponentInChildren<TextMeshProUGUI>().text = player3;
    }
    public void OnPauseBtnClk()
    {

        UIManager.Instance.ChangeScreen(SceneName.PausedGamePlay);
        // UIAnimationManager.instance.Animate(AnimationMenu.PausedGamePlay);
        GameStateManager.Instance.ChangeGameState(GameState.PausedGamePlay);
        Time.timeScale = 0;

    }
    void OnGameStateChange(GameState currentgameState)
    {
        if (currentgameState == GameState.GamePlay)
        {
            Time.timeScale = 1;
        }
    }
    public void OnScoreChanged(int playerScore, int AIScore)
    {

        // if (!PlayerPrefs.HasKey("Score"))
        // {
        //     PlayerPrefs.SetInt("Score", 0);
        // }

        /*string[] text = ScoreTextInGame.text.Split(char.Parse(":"));
        ScoreTextInGame.text = text[0] + ":" + playerScore.ToString();*/

        // 

    }
    public void showTurnImage(int index)
    {
        if (tempIndex == -1)
            tempIndex = index;
        else
        {
            hideTurnImage(tempIndex);
            tempIndex = index;
        }
        Debug.Log("Timer started for index " + index);
        Timer = StartCoroutine(ShowTurnImageTimer(index));
    }
    public void hideTurnImage(int index)
    {
        Debug.Log("Timer stopped for index " + index);
        StopCoroutine(Timer);
        TurnImages[index].fillAmount = 0;
    }
    IEnumerator ShowTurnImageTimer(int index)
    {
        float elapsedTime = 0;
        float waitTime = 60f;

        while (elapsedTime < waitTime)
        {
            TurnImages[index].fillAmount = Mathf.Lerp(1, 0, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;

            // Yield here
            yield return null;
        }
        yield return null;
    }
    public void OnGameModeChange(GameMode currentMode)
    {
        Debug.Log(currentMode);
        Debug.Log("Mode Change");
        switch (currentMode)
        {
            case GameMode.LearnToPlay:
                // Timer.enabled = false;
                // ScoreTextInGame.enabled = false;


                // GameManagerScript.instance.AIPlayerDisable();
                //remove Timer
                // Debug.Log("In Gameplay -> FreePractice");
                break;
            case GameMode.OnlineMultiplayer:
                // Timer.enabled = true;
                // ScoreTextInGame.enabled = true;
                // ScoreTimer.Instance.startInitialTimer();

                // GameManagerScript.instance.AIPlayerDisable();
                //set timer 
                // Debug.Log("In Gameplay -> TimeTrails");
                break;
            case GameMode.PlayerVsAI:
                // Timer.enabled = true;
                // ScoreTextInGame.enabled = true;
                // ScoreTimer.Instance.startInitialTimer();

                // GameManagerScript.instance.AIPlayerDisable();
                //set timer 
                // Debug.Log("In Gameplay -> TimeTrails");
                break;
        }

    }

}
