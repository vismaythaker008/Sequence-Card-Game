using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SequenceCardGame;

public class CardsManagerScript : MonoBehaviour
{
    public static CardsManagerScript instance;
    public MatrixOfCards[] GridMatrixOfTotalDisplayCards;
    public List<MatrixOfCards> ListOfGridMatrixOfTotalDisplayCards;
    public delegate void TotalCardCountFound();

    //event  
    public static event TotalCardCountFound OnTotalCardFound;


    public GameObject[] Cards;
    public List<DeckOfCards> TotalCards;

    public int PlayerCount;
    public int TotalCardCount;
    public int deckCount;


    void Awake()
    {
        instance = this;

    }
    void Start()
    {
        ListOfGridMatrixOfTotalDisplayCards = new List<MatrixOfCards>(GridMatrixOfTotalDisplayCards);

        for (int i = 0; i < Cards.Length; i++)
        {
            for (int j = 1; j < deckCount + 1; j++)
            {
                DeckOfCards deckOfCard = new DeckOfCards();
                deckOfCard.Card = Cards[i];
                deckOfCard.deck = j;
                TotalCards.Add(deckOfCard);
            }
        }

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
        GameObject card;
        int tempIndex;
        do
        {
            tempIndex = Random.Range(0, TotalCards.Count);
            card = TotalCards[tempIndex].Card;

        } while (TotalCards[tempIndex].checkAssigned());
        TotalCards[tempIndex].assign();
        return card;
    }

}
[System.Serializable]
public class MatrixOfCards
{
    public int row;
    public int column;
    public int deck = 0;
    public GameObject Chip = null;
    public GameObject Card;

}
[System.Serializable]
public class DeckOfCards
{
    public GameObject Card;
    public int deck;
    private bool assigned = false;

    public bool checkAssigned()
    {
        return assigned;
    }
    public void assign()
    {
        assigned = true;
    }

}
