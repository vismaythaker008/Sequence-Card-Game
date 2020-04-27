using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AI : MonoBehaviour
{
    #region variables
    public List<GameObject> Cards;
    public GameObject selectedChip;
    private Coroutine PutChip;


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
    private int JackCardIndex = -1;

    #endregion
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
                matrixOfCards.row = 100;
                matrixOfCards.column = 100;
                matrixOfCards.Card = Card;
                MatrixOfOwnCards.Add(matrixOfCards);
                index++;
            }
            else
            {

                MatrixOfOwnCards.Add(CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards.Find(x => x.Card.name == Card.name && x.deck == 1));
                Debug.Log(" index " + index + " card " + MatrixOfOwnCards[index].Card + " row " + MatrixOfOwnCards[index].row + " column " + MatrixOfOwnCards[index].column);
                index++;

                MatrixOfOwnCards.Add(CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards.Find(x => x.Card.name == Card.name && x.deck == 2));
                Debug.Log(" index " + index + " card " + MatrixOfOwnCards[index].Card + " row " + MatrixOfOwnCards[index].row + " column " + MatrixOfOwnCards[index].column);
                index++;
            }
            // Debug.Log(Cards[CardCount]);
            // Debug.Log(MatrixOfOwnCards.Count);
            CardCount++;
        }
        yield return null;
    }

    IEnumerator play()
    {
        int index;

        if ((index = MatrixOfOwnCards.FindIndex(o => o.Card.name.Contains("Jack"))) != -1)
        {
            FindCardForJacks(index);
        }
        // breaking other players pairs
        else if ((index = BreakOtherPlayersPairs()) != -1 && checkIfPossible(index))
        {
            PutCard(index, false);
        }

        //near to previous card
        else if ((index = getNearestNeighbour()) != -1 && checkIfPossible(index))
        {
            PutCard(index, false);
        }
        //near to corner
        else if ((index = nearToCorner()) != -1 && checkIfPossible(index))
        {
            PutCard(index, false);
        }
        else if ((index = nearToCorner()) != -1 && checkIfPossible(index))
        {
            PutCard(index, false);
        }
        else
        {
            int tempIndex;
            bool notSelected = true;
            do
            {
                tempIndex = Random.Range(0, MatrixOfOwnCards.Count);
                if (!MatrixOfOwnCards[tempIndex].Card.name.Contains("Jack") && checkIfPossible(tempIndex))
                {
                    notSelected = false;
                }
            } while (notSelected);

            PutCard(tempIndex, false);

        }

        yield return null;
    }
    public void FindCardForJacks(int index)
    {
        JackCardIndex = index;
        MatrixOfCards matrixOfCard = MatrixOfOwnCards[index];
        SolutionInfo solutionInfo;
        switch (matrixOfCard.Card.name)
        {
            case "JackofSpades":
            case "JackofHearts":
                Debug.Log("one eyed");
                int tempIndex = -1;
                int row = -1;
                int column = -1;

                solutionInfo = ScoreManagerScript.instance.getOtherPlayersScore(playerIndex);
                do
                {
                    tempIndex = Random.Range(0, solutionInfo.data.Count);
                    row = solutionInfo.data[tempIndex].row;
                    column = solutionInfo.data[tempIndex].column;
                } while ((row == 0 && column == 0) || (row == 0 && column == 9) || (row == 9 && column == 0) || (row == 9 && column == 9));


                Debug.Log(tempIndex + "  " + row + "  " + column);
                PutCard(CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards.FindIndex(o => o.row == row && o.column == column), true);
                // ScoreManagerScript.instance.ScoreOfCards[solutionInfo.data[Random.Range(0, solutionInfo.data.Count)].row, solutionInfo.data[Random.Range(0, solutionInfo.data.Count)].column] = -1;

                break;
            case "JackofClubs":
            case "JackofDiamonds":
                Debug.Log("2 eyed");
                solutionInfo = ScoreManagerScript.instance.getOtherPlayersScore(playerIndex);
                if (solutionInfo.length > 3)
                    ScoreManagerScript.instance.ScoreOfCards[solutionInfo.data[Random.Range(0, solutionInfo.data.Count)].row, solutionInfo.data[Random.Range(0, solutionInfo.data.Count)].column] = playerIndex;
                else
                {

                    if ((index = ExtendSequenceByJack()) != -1)
                    {
                        PutCard(index, true);
                    }
                }
                break;

        }
    }
    int ExtendSequenceByJack()
    {
        int nearestIndex = -1;
        for (int i = 0; i < CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards.Count; i++)
        {
            if (ScoreManagerScript.instance.ScoreOfCards[CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards[i].row, CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards[i].column] == -1)
            {

                ScoreManagerScript.instance.ScoreOfCards[CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards[i].row, CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards[i].column] = playerIndex;

                if (Solution.longestLine(ScoreManagerScript.instance.ScoreOfCards, playerIndex).length == 4)
                {
                    nearestIndex = i;
                }
                else if (Solution.longestLine(ScoreManagerScript.instance.ScoreOfCards, playerIndex).length == 3)
                {
                    nearestIndex = i;
                }
                else if (Solution.longestLine(ScoreManagerScript.instance.ScoreOfCards, playerIndex).length == 2)
                {
                    nearestIndex = i;
                }
                else
                {
                    nearestIndex = -1;
                }
                ScoreManagerScript.instance.ScoreOfCards[CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards[i].row, CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards[i].column] = -1;
            }
        }
        return nearestIndex;
    }
    bool checkIfPossible(int index)
    {
        if (ScoreManagerScript.instance.ScoreOfCards[MatrixOfOwnCards[index].row, MatrixOfOwnCards[index].column] == -1)
            return true;
        else
            return false;
    }

    public void PutCard(int index, bool jack)
    {

        selectedChip = manageChip.getChip();

        // Debug.Log(selectedChip);
        //        Debug.Log(" put card index " + index + " card " + MatrixOfOwnCards[index].Card + " row " + MatrixOfOwnCards[index].row + " column " + MatrixOfOwnCards[index].column);
        PutChip = StartCoroutine(putChip(selectedChip, index, jack));
        if (!jack)
        {
            int i = CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards.FindIndex(o => o.Card.Equals(MatrixOfOwnCards[index].Card) && o.deck == MatrixOfOwnCards[index].deck);
            Debug.Log(i);
            changeCard(MatrixOfOwnCards[index].Card);
            index = i;
        }
        else
        {
            changeCard(MatrixOfOwnCards[JackCardIndex].Card);
            JackCardIndex = -1;
        }
        CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards[index].Chip = selectedChip;
        ScoreManagerScript.instance.updateScore(CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards[index].Card, false);

        //ScoreManagerScript.instance.ScoreOfCards[MatrixOfOwnCards[index].row, MatrixOfOwnCards[index].column] = playerIndex;

        GameManagerScript.instance.endRound();
    }
    public void changeCard(GameObject card)
    {
        // int cardPositionIndexInList = Cards.FindIndex(o => o == card);
        int cardPositionIndexInMatrix;
        string name = "";
        Cards.Remove(card);


        GameObject Card = CardsManagerScript.instance.getCard();
        Debug.Log(Card.name);

        Cards.Add(Card);
        if (JackCardIndex != -1)
        {
            name = MatrixOfOwnCards[JackCardIndex].Card.name;
        }
        else
        {
            name = card.name;
        }
        if (JackCardIndex != -1)
        {
            cardPositionIndexInMatrix = MatrixOfOwnCards.FindIndex(o => o.Card.name == name);
            MatrixOfOwnCards.Remove(MatrixOfOwnCards[cardPositionIndexInMatrix]);

            if (Card.name.Contains("Jack"))
            {

                MatrixOfCards matrixOfCard = new MatrixOfCards();
                matrixOfCard.row = 100;
                matrixOfCard.column = 100;
                matrixOfCard.Card = Card;
                MatrixOfOwnCards.Add(matrixOfCard);
            }
            else
            {

                MatrixOfOwnCards.Add(CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards.Find(x => x.Card.name == Card.name && x.deck == 1));

                MatrixOfOwnCards.Add(CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards.Find(x => x.Card.name == Card.name && x.deck == 2));
                // MatrixOfOwnCards.Add(GridMatrixOfTotalDisplayCards.Find(x => x.Card.name == Card.name));
                int tempIndex = MatrixOfOwnCards.FindIndex(o => o.Card.name == Card.name && o.deck == 1);
                Debug.Log(" index " + tempIndex + " card " + MatrixOfOwnCards[tempIndex].Card + " row " + MatrixOfOwnCards[tempIndex].row + " column " + MatrixOfOwnCards[tempIndex].column);

                // MatrixOfOwnCards.Add(GridMatrixOfTotalDisplayCards.FindLast(x => x.Card.name == Card.name));
                tempIndex = MatrixOfOwnCards.FindIndex(o => o.Card.name == Card.name && o.deck == 2);
                Debug.Log(" index " + tempIndex + " card " + MatrixOfOwnCards[tempIndex].Card + " row " + MatrixOfOwnCards[tempIndex].row + " column " + MatrixOfOwnCards[tempIndex].column);

            }

        }
        else
        {
            cardPositionIndexInMatrix = MatrixOfOwnCards.FindIndex(o => o.Card.name == name && o.deck == 1);
            MatrixOfOwnCards.Remove(MatrixOfOwnCards[cardPositionIndexInMatrix]);
            cardPositionIndexInMatrix = MatrixOfOwnCards.FindIndex(o => o.Card.name == name && o.deck == 2);
            MatrixOfOwnCards.Remove(MatrixOfOwnCards[cardPositionIndexInMatrix]);
            if (Card.name.Contains("Jack"))
            {

                MatrixOfCards matrixOfCard = new MatrixOfCards();
                matrixOfCard.row = 100;
                matrixOfCard.column = 100;
                matrixOfCard.Card = Card;
                MatrixOfOwnCards.Add(matrixOfCard);
            }
            else
            {

                MatrixOfOwnCards.Add(CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards.Find(x => x.Card.name == Card.name && x.deck == 1));

                MatrixOfOwnCards.Add(CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards.Find(x => x.Card.name == Card.name && x.deck == 2));
                // MatrixOfOwnCards.Add(GridMatrixOfTotalDisplayCards.Find(x => x.Card.name == Card.name));
                int tempIndex = MatrixOfOwnCards.FindIndex(o => o.Card.name == Card.name && o.deck == 1);
                Debug.Log(" index " + tempIndex + " card " + MatrixOfOwnCards[tempIndex].Card + " row " + MatrixOfOwnCards[tempIndex].row + " column " + MatrixOfOwnCards[tempIndex].column);

                // MatrixOfOwnCards.Add(GridMatrixOfTotalDisplayCards.FindLast(x => x.Card.name == Card.name));
                tempIndex = MatrixOfOwnCards.FindIndex(o => o.Card.name == Card.name && o.deck == 2);
                Debug.Log(" index " + tempIndex + " card " + MatrixOfOwnCards[tempIndex].Card + " row " + MatrixOfOwnCards[tempIndex].row + " column " + MatrixOfOwnCards[tempIndex].column);

            }

        }



    }
    IEnumerator putChip(GameObject chip, int index, bool jack)
    {
        float elapsedTime = 0;
        float waitTime = 0.3f;
        Vector3 newPosition;
        if (jack)
        {
            newPosition = CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards[index].Card.transform.position;
        }
        else
        {
            newPosition = MatrixOfOwnCards[index].Card.transform.position;
        }

        newPosition.y += 0.3f;
        while (elapsedTime < waitTime)
        {
            chip.transform.position = Vector3.Lerp(chip.transform.position, newPosition, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;

            // Yield here
            yield return new WaitForEndOfFrame();
        }
        // Make sure we got there
        chip.transform.position = newPosition;
        yield return null;
    }
    public int BreakOtherPlayersPairs()
    {
        int nearestIndex = -1;

        for (int i = 0; i < MatrixOfOwnCards.Count; i++)
        {
            if (MatrixOfOwnCards[i].row != 100)
            {
                int temp = ScoreManagerScript.instance.ScoreOfCards[MatrixOfOwnCards[i].row, MatrixOfOwnCards[i].column];
                ScoreManagerScript.instance.ScoreOfCards[MatrixOfOwnCards[i].row, MatrixOfOwnCards[i].column] = playerIndex;

                if (Solution.longestLine(ScoreManagerScript.instance.ScoreOfCards, playerIndex).length == 4)
                {
                    nearestIndex = i;
                }
                else if (Solution.longestLine(ScoreManagerScript.instance.ScoreOfCards, playerIndex).length == 3)
                {
                    nearestIndex = i;
                }
                else if (Solution.longestLine(ScoreManagerScript.instance.ScoreOfCards, playerIndex).length == 2)
                {
                    nearestIndex = i;
                }
                else
                {
                    nearestIndex = -1;
                }
                ScoreManagerScript.instance.ScoreOfCards[MatrixOfOwnCards[i].row, MatrixOfOwnCards[i].column] = temp;
            }
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

                // Debug.Log("i=" + i + " j=" + j);
                // Debug.Log(MatrixOfOwnCards[i].Card.name);
                // Debug.Log(MatrixOfOwnCards[i].row);
                // Debug.Log(MatrixOfOwnCards[i].column);
                // Debug.Log(row[j] + " " + column[j]);
                if (Mathf.Abs(MatrixOfOwnCards[i].row - row[j]) < 6 && Mathf.Abs(MatrixOfOwnCards[i].column - column[j]) == 0)
                {
                    // Debug.Log("01");
                    if (Mathf.Min(minimumDistance, Mathf.Abs(MatrixOfOwnCards[i].row - row[j])) == Mathf.Abs(MatrixOfOwnCards[i].row - row[j]))
                    {
                        nearestIndex = i;
                        minimumDistance = Mathf.Abs(MatrixOfOwnCards[i].row - row[j]);
                    }
                    // Data.Add(i, Mathf.Abs(MatrixOfOwnCards[i].row - row[j]));
                }
                else if (Mathf.Abs(MatrixOfOwnCards[i].column - column[j]) < 6 && Mathf.Abs(MatrixOfOwnCards[i].row - row[j]) == 0)
                {

                    // Debug.Log("02");
                    if (Mathf.Min(minimumDistance, Mathf.Abs(MatrixOfOwnCards[i].column - column[j])) == Mathf.Abs(MatrixOfOwnCards[i].column - column[j]))
                    {
                        nearestIndex = i;
                        minimumDistance = Mathf.Abs(MatrixOfOwnCards[i].column - column[j]);
                    }
                    // Data.Add(i, Mathf.Abs(MatrixOfOwnCards[i].column - column[j]));

                }
                else if (Mathf.Abs(MatrixOfOwnCards[i].row - row[j]) < 6 && Mathf.Abs(MatrixOfOwnCards[i].column - column[j]) < 6 && Mathf.Abs(MatrixOfOwnCards[i].row - row[j]) == Mathf.Abs(MatrixOfOwnCards[i].column - column[j]))
                {
                    // Debug.Log("03");
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

