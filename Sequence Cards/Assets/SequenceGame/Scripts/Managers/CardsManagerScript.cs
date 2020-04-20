using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SequenceCardGame;

public class CardsManagerScript : MonoBehaviour
{
    public static CardsManagerScript instance;
    public MatrixOfCards[] GridMatrixOfTotalDisplayCards;
    public int[,] ScoreOfCards;

    public delegate void TotalCardCountFound();

    //event  
    public static event TotalCardCountFound OnTotalCardFound;

    public Player player;
    public GameObject[] Cards;

    public int PlayerCount;
    public int TotalCardCount;



    void Awake()
    {
        instance = this;
        ScoreOfCards = new int[10, 10];
        // Debug.Log(ScoreOfCards.Length);
        for (int i = 0; i < 10; i++)
        {
            // ScoreOfCards[i] = new int[10];
            for (int j = 0; j < 10; j++)
            {
                ScoreOfCards[i, j] = -1;
                // Debug.Log("i,j=" + i + "," + j + " =" + ScoreOfCards[i, j]);
            }
        }
        ScoreOfCards[0, 0] = -2;
        ScoreOfCards[9, 0] = -2;
        ScoreOfCards[0, 9] = -2;
        ScoreOfCards[9, 9] = -2;
    }

    public void setPlayerCount(int count)
    {

        PlayerCount = count;
        switch (PlayerCount)
        {
            case 2:
                TotalCardCount = 7;
                break;
            case 3:
                TotalCardCount = 6;
                break;
            case 4:
                TotalCardCount = 6;
                break;
            case 6:
                TotalCardCount = 5;
                break;
            case 8:
                TotalCardCount = 4;
                break;
            case 9:
                TotalCardCount = 4;
                break;
            case 10:
                TotalCardCount = 3;
                break;
            case 12:
                TotalCardCount = 3;
                break;

        }
        // Utilities.WaitAsync(1000, () =>
        //              {
        OnTotalCardFound();
        //              });

    }

    public GameObject getCard()
    {
        return Cards[Random.Range(0, Cards.Length)];
    }







}
[System.Serializable]
public class MatrixOfCards
{
    public int row;
    public int column;
    public GameObject Card;

}
