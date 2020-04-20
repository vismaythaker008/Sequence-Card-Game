using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SequenceCardGame;

public class CardsManagerScript : MonoBehaviour
{
    public static CardsManagerScript instance;
    public MatrixOfCards[] GridMatrixOfTotalDisplayCards;

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
