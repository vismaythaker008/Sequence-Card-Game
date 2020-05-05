using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SequenceCardGame;


public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance;
    public GameplayUI gameplayUI;
    public GameObject CardsLeft;
    public GameObject CardsRight;

    public int PlayerCount;
    // public

    public GameObject[] Players;
    public GameObject playerPrefab;
    public GameObject AIPrefab;
    public Vector3[] ChipPositions;
    public int curentPlayerIndex;
    public delegate void OnTurnChange(int index);
    public static event OnTurnChange TurnChanged;
    public delegate void OnTurnImageShow(int index);
    public static event OnTurnImageShow ShowTurnImage;

    private string[] chipTags = { ConstantString.TagForPlayingChipsTeam1, ConstantString.TagForPlayingChipsTeam2, ConstantString.TagForPlayingChipsTeam3 };
    private int startingPlayerIndex = -1;
    private int turn = 0;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {

        AudioManager.instance.Play("BG Music");

    }

    void play()
    {
        curentPlayerIndex = turn;
        if (TurnChanged != null)
            TurnChanged(turn);
        if (ShowTurnImage != null)
            ShowTurnImage(turn);
    }



    public void endRound()
    {
        Debug.Log("end Round");
        ScoreManagerScript.instance.CheckScore();

        if (turn < PlayerCount)
        {
            turn++;
        }
        if (turn == PlayerCount)
        {
            turn = 0;
        }
        play();
    }

    public void startGame()
    {
        startingPlayerIndex = Random.Range(0, PlayerCount);
        Debug.Log("startingPlayerIndex : " + startingPlayerIndex);
        Utilities.WaitAsync(1200, () =>
        {

            if (startingPlayerIndex != -1)
            {
                turn = startingPlayerIndex;
                startingPlayerIndex = -1;
            }
            play();
        });

    }
    public void endGame()
    {


    }
    public void setPlayerCount(int count)
    {
        PlayerCount = count;
        ScoreManagerScript.instance.setUpScoreInfo(count);
        // Debug.Log("123");
        ChipsManager.instance.setPlayerCount(count);
        createPlayers(PlayerCount);
        // Debug.Log("456");
    }
    void createPlayers(int count)
    {
        Transform child;
        Players = new GameObject[count];
        Players[0] = Instantiate(playerPrefab, transform);
        Players[0].name = playerPrefab.name;
        Players[0].GetComponent<Player>().playerIndex = 0;
        child = Players[0].transform.GetChild(Players[0].transform.childCount - 1);
        child.position = ChipPositions[0];
        Players[0].GetComponentInChildren<ManageChip>().setchipTag(chipTags[0]);
        Players[0].GetComponentInChildren<ManageChip>().callManageChips();

        for (int i = 1; i < count; i++)
        {
            Players[i] = Instantiate(AIPrefab, transform);
            Players[i].name = AIPrefab.name;
            Players[i].GetComponent<AI>().playerIndex = i;
            child = Players[i].transform.GetChild(Players[i].transform.childCount - 1);
            child.position = ChipPositions[i];
            Players[i].GetComponentInChildren<ManageChip>().setchipTag(chipTags[i]);
            Players[i].GetComponentInChildren<ManageChip>().callManageChips();
        }
    }
    public void makePlayersReady()
    {
        Players[0].GetComponent<Player>().ShowCards();

        for (int i = 1; i < PlayerCount; i++)
        {
            Players[i].GetComponent<AI>().ShowCards();
        }
    }
    public void ShowCards()
    {
        CardsLeft.SetActive(true);
        CardsRight.SetActive(true);
    }
    public void HideCards()
    {
        CardsLeft.SetActive(false);
        CardsRight.SetActive(false);
    }






}
