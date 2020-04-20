using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AI : MonoBehaviour
{
    public List<GameObject> Cards;
    public GameObject selectedChip;
    private Coroutine PutChip;
    public List<MatrixOfCards> GridMatrixOfTotalDisplayCards;
    public int[,] ScoreOfCards;
    public List<MatrixOfCards> MatrixOfOwnCards;
    public MatrixOfCards[] MatrixOfUsedCards;

    private GameObject[] Players;
    private Coroutine Play;
    private Coroutine ManageCards;
    public int CardCount = 0;
    private int index = 0;
    public int TotalCardCount;
    private ManageChip manageChip;
    public int playerIndex;

    void Start()
    {
        ScoreOfCards = CardsManagerScript.instance.ScoreOfCards;
        GridMatrixOfTotalDisplayCards = new List<MatrixOfCards>(CardsManagerScript.instance.GridMatrixOfTotalDisplayCards);

    }
    void OnEnable()
    {
        CardsManagerScript.OnTotalCardFound += setTotalCardCount;
        GameManagerScript.TurnChanged += startGame;
    }

    void OnDisable()
    {
        CardsManagerScript.OnTotalCardFound -= setTotalCardCount;
        GameManagerScript.TurnChanged -= startGame;
    }
    void setTotalCardCount()
    {
        TotalCardCount = CardsManagerScript.instance.TotalCardCount;

    }

    IEnumerator ManageCard()
    {

        while (CardCount < TotalCardCount)
        {
            GameObject Card = CardsManagerScript.instance.getCard();
            Cards.Add(Card);
            Debug.Log(Card.name);
            if (Card.name.Contains("Jack"))
            {
                MatrixOfCards matrixOfCards = new MatrixOfCards();
                matrixOfCards.row = 0;
                matrixOfCards.column = 0;
                matrixOfCards.Card = Card;
                MatrixOfOwnCards.Add(matrixOfCards);
                index++;
            }
            else
            {

                MatrixOfOwnCards.Add(GridMatrixOfTotalDisplayCards.Find(x => x.Card.name == Card.name));
                Debug.Log(" index " + index + " card " + MatrixOfOwnCards[index].Card + " row " + MatrixOfOwnCards[index].row + " column " + MatrixOfOwnCards[index].column);
                index++;

                MatrixOfOwnCards.Add(GridMatrixOfTotalDisplayCards.FindLast(x => x.Card.name == Card.name));
                Debug.Log(" index " + index + " card " + MatrixOfOwnCards[index].Card + " row " + MatrixOfOwnCards[index].row + " column " + MatrixOfOwnCards[index].column);
                index++;
            }
            Debug.Log(Cards[CardCount]);
            Debug.Log(MatrixOfOwnCards.Count);
            CardCount++;
        }
        yield return null;
    }

    IEnumerator play()
    {
        int index;

        //breaking other players pairs
        // if ((index = BreakOtherPlayersPairs()) != -1)
        // {
        //     PutCard(index);
        // }

        // //near to previous card
        // else if ((index = getNearestNeighbour()) != -1)
        // {
        //     PutCard(index);
        // }
        // //near to corner
        // else if ((index = nearToCorner()) != -1)
        // {
        //     PutCard(index);
        // }
        if ((index = nearToCorner()) != -1)
        {
            PutCard(index);
        }
        else
        {
            PutCard(Random.Range(0, MatrixOfOwnCards.Count));
        }

        yield return null;
    }
    public void PutCard(int index)
    {
        if (selectedChip == null)
            selectedChip = manageChip.getChip();

        Debug.Log(selectedChip);
        Debug.Log(" put card index " + index + " card " + MatrixOfOwnCards[index].Card + " row " + MatrixOfOwnCards[index].row + " column " + MatrixOfOwnCards[index].column);
        PutChip = StartCoroutine(putChip(selectedChip, index));
        GameManagerScript.instance.endRound();
    }
    IEnumerator putChip(GameObject chip, int index)
    {
        float elapsedTime = 0;
        float waitTime = 1.5f;
        while (elapsedTime < waitTime)
        {
            transform.position = Vector3.Lerp(chip.transform.position, MatrixOfOwnCards[index].Card.transform.position, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;

            // Yield here
            yield return null;
        }
        // Make sure we got there
        transform.position = MatrixOfOwnCards[index].Card.transform.position;
        yield return null;
    }
    public int BreakOtherPlayersPairs()
    {
        int nearestIndex = -1;
        for (int i = 0; i < MatrixOfOwnCards.Count; i++)
        {
            int temp = ScoreOfCards[MatrixOfOwnCards[i].row, MatrixOfOwnCards[i].column];
            ScoreOfCards[MatrixOfOwnCards[i].row, MatrixOfOwnCards[i].column] = 1;

            if (Solution.longestLine(ScoreOfCards, 1) == 4)
            {
                nearestIndex = i;
            }
            else if (Solution.longestLine(ScoreOfCards, 1) == 3)
            {
                nearestIndex = i;
            }
            else if (Solution.longestLine(ScoreOfCards, 1) == 2)
            {
                nearestIndex = i;
            }
            else
            {
                nearestIndex = -1;
            }
            ScoreOfCards[MatrixOfOwnCards[i].row, MatrixOfOwnCards[i].column] = temp;
        }
        return nearestIndex;
    }
    public int getNearestNeighbour()
    {

        // int[] DataLength = new int[MatrixOfOwnCards.Length];
        int nearestIndex = -1;
        int minimumDistance = 100;

        // Dictionary<int, int> Data = new Dictionary<int, int>();
        for (int i = 0; i < MatrixOfOwnCards.Count; i++)
        {
            for (int j = 0; j < MatrixOfUsedCards.Length; j++)
            {
                if (Mathf.Abs(MatrixOfOwnCards[i].row - MatrixOfUsedCards[j].row) < 6 && Mathf.Abs(MatrixOfOwnCards[i].column - MatrixOfUsedCards[j].column) == 0)
                {
                    if (Mathf.Min(minimumDistance, Mathf.Abs(MatrixOfOwnCards[i].row - MatrixOfUsedCards[j].row)) == Mathf.Abs(MatrixOfOwnCards[i].row - MatrixOfUsedCards[j].row))
                    {
                        nearestIndex = i;
                        minimumDistance = Mathf.Abs(MatrixOfOwnCards[i].row - MatrixOfUsedCards[j].row);
                    }
                    // Data.Add(i, Mathf.Abs(MatrixOfOwnCards[i].row - MatrixOfUsedCards[j].row));
                }
                else if (Mathf.Abs(MatrixOfOwnCards[i].column - MatrixOfUsedCards[j].column) < 6 && Mathf.Abs(MatrixOfOwnCards[i].row - MatrixOfUsedCards[j].row) == 0)
                {
                    if (Mathf.Min(minimumDistance, Mathf.Abs(MatrixOfOwnCards[i].column - MatrixOfUsedCards[j].column)) == Mathf.Abs(MatrixOfOwnCards[i].column - MatrixOfUsedCards[j].column))
                    {
                        nearestIndex = i;
                        minimumDistance = Mathf.Abs(MatrixOfOwnCards[i].column - MatrixOfUsedCards[j].column);
                    }
                    // Data.Add(i, Mathf.Abs(MatrixOfOwnCards[i].column - MatrixOfUsedCards[j].column));
                }
                else if (Mathf.Abs(MatrixOfOwnCards[i].row - MatrixOfUsedCards[j].row) > 0 && Mathf.Abs(MatrixOfOwnCards[i].column - MatrixOfUsedCards[j].column) > 0 && Mathf.Abs(MatrixOfOwnCards[i].row - MatrixOfUsedCards[j].row) == Mathf.Abs(MatrixOfOwnCards[i].column - MatrixOfUsedCards[j].column))
                {
                    if (Mathf.Min(minimumDistance, Mathf.Abs(MatrixOfOwnCards[i].row - MatrixOfUsedCards[j].row)) == Mathf.Abs(MatrixOfOwnCards[i].row - MatrixOfUsedCards[j].row))
                    {
                        nearestIndex = i;
                        minimumDistance = Mathf.Abs(MatrixOfOwnCards[i].row - MatrixOfUsedCards[j].row);
                    }
                    // Data.Add(i, Mathf.Abs(MatrixOfOwnCards[i].row - MatrixOfUsedCards[j].row));
                }
            }
        }

        // foreach (int key in Data.Keys)
        // {
        //     DataLength[index] = key;
        //     index++;
        // }

        // int nearestIndex = Utilities.KeyByValue(Data, Mathf.Min(DataLength));
        return nearestIndex;

    }
    public int nearToCorner()
    {
        // int index = 0;
        int[] row = { 0, 0, 9, 9 };
        int[] column = { 0, 9, 9, 0 };
        int[] DataLength = new int[MatrixOfOwnCards.Count];
        int minimumDistance = 100;
        int nearestIndex = -1;

        Dictionary<int, int> Data = new Dictionary<int, int>();
        for (int i = 0; i < MatrixOfOwnCards.Count; i++)
        {
            for (int j = 0; j < 4; j++)
            {

                Debug.Log("i=" + i + " j=" + j);
                Debug.Log(MatrixOfOwnCards[i].Card.name);
                Debug.Log(MatrixOfOwnCards[i].row);
                Debug.Log(MatrixOfOwnCards[i].column);
                Debug.Log(row[j] + " " + column[j]);
                if (Mathf.Abs(MatrixOfOwnCards[i].row - row[j]) < 6 && Mathf.Abs(MatrixOfOwnCards[i].column - column[j]) == 0)
                {
                    Debug.Log("01");
                    if (Mathf.Min(minimumDistance, Mathf.Abs(MatrixOfOwnCards[i].row - row[j])) == Mathf.Abs(MatrixOfOwnCards[i].row - row[j]))
                    {
                        nearestIndex = i;
                        minimumDistance = Mathf.Abs(MatrixOfOwnCards[i].row - row[j]);
                    }
                    // Data.Add(i, Mathf.Abs(MatrixOfOwnCards[i].row - row[j]));
                }
                else if (Mathf.Abs(MatrixOfOwnCards[i].column - column[j]) < 6 && Mathf.Abs(MatrixOfOwnCards[i].row - row[j]) == 0)
                {

                    Debug.Log("02");
                    if (Mathf.Min(minimumDistance, Mathf.Abs(MatrixOfOwnCards[i].column - column[j])) == Mathf.Abs(MatrixOfOwnCards[i].column - column[j]))
                    {
                        nearestIndex = i;
                        minimumDistance = Mathf.Abs(MatrixOfOwnCards[i].column - column[j]);
                    }
                    // Data.Add(i, Mathf.Abs(MatrixOfOwnCards[i].column - column[j]));

                }
                else if (Mathf.Abs(MatrixOfOwnCards[i].row - row[j]) < 6 && Mathf.Abs(MatrixOfOwnCards[i].column - column[j]) < 6 && Mathf.Abs(MatrixOfOwnCards[i].row - row[j]) == Mathf.Abs(MatrixOfOwnCards[i].column - column[j]))
                {
                    Debug.Log("03");
                    if (Mathf.Min(minimumDistance, Mathf.Abs(MatrixOfOwnCards[i].column - column[j])) == Mathf.Abs(MatrixOfOwnCards[i].column - column[j]))
                    {
                        nearestIndex = i;
                        minimumDistance = Mathf.Abs(MatrixOfOwnCards[i].column - column[j]);
                    }
                    // Data.Add(i, Mathf.Abs(Mathf.Abs(MatrixOfOwnCards[i].column - column[j])));
                }
            }
        }

        // foreach (int value in Data.Values)
        // {
        //     DataLength[index] = value;
        //     index++;
        // }
        // nearestIndex = Utilities.KeyByValue(Data, Mathf.Min(DataLength));
        return nearestIndex;
    }

    public void startGame(int index)
    {
        if (playerIndex == index)
        {
            Debug.Log("start game AI");
            manageChip = transform.GetComponentInChildren<ManageChip>();
            Play = StartCoroutine(play());
        }
        else
        {
            endGame();
        }


    }
    public void endGame()
    {
        Debug.Log("end game ai");
        if (Play != null)
            StopCoroutine(Play);
    }



    public void ShowCards()
    {

        ManageCards = StartCoroutine(ManageCard());

    }
    public void HideCards()
    {
        StopCoroutine(ManageCards);


    }

}

