using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AI : MonoBehaviour
{
    #region variables
    public List<GameObject> Cards;
    public float Speed = 10f;
    public GameObject selectedChip;
    private Coroutine PutChip;


    public List<MatrixOfCards> MatrixOfOwnCards;
    public List<MatrixOfCards> MatrixOfUsedCards;
    public string JackCardName = "";
    private GameObject[] Players;
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

    void ManageCard()
    {
        Debug.Log("ManageCard");
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

    }

    void Play()
    {
        int index;
        JackCardName = "";
        // 
        // breaking other players pairs
        if ((index = BreakOtherPlayersPairs()) != -1 && checkIfPossible(index))
        {
            index = CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards.FindIndex(c => c.row == MatrixOfOwnCards[index].row && c.column == MatrixOfOwnCards[index].column && c.deck == MatrixOfOwnCards[index].deck);
            PutCard(index);
        }

        //near to previous card
        else if ((index = getNearestNeighbour()) != -1 && checkIfPossible(index))
        {
            index = CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards.FindIndex(c => c.row == MatrixOfOwnCards[index].row && c.column == MatrixOfOwnCards[index].column && c.deck == MatrixOfOwnCards[index].deck);
            PutCard(index);
        }
        else if ((index = MatrixOfOwnCards.FindIndex(o => o.Card.name.Contains("Jack"))) != -1 && FindCardForJacks(index) != -1 && checkIfPossible(index))
        {

            PutCard(index);
        }
        //near to corner
        else if ((index = nearToCorner()) != -1 && checkIfPossible(index))
        {
            index = CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards.FindIndex(c => c.row == MatrixOfOwnCards[index].row && c.column == MatrixOfOwnCards[index].column && c.deck == MatrixOfOwnCards[index].deck);
            PutCard(index);
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
            index = CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards.FindIndex(c => c.row == MatrixOfOwnCards[tempIndex].row && c.column == MatrixOfOwnCards[tempIndex].column && c.deck == MatrixOfOwnCards[tempIndex].deck);
            PutCard(index);

        }


    }
    public int FindCardForJacks(int index)
    {
        Debug.Log("FindCardForJacks");
        JackCardIndex = index;
        MatrixOfCards matrixOfCard = MatrixOfOwnCards[index];
        SolutionInfo solutionInfo;
        int tempIndex = -1;
        int row = -1;
        int column = -1;
        switch (matrixOfCard.Card.name)
        {
            case "JackofSpades":
            case "JackofHearts":
                Debug.Log("one eyed");

                JackCardName = "one eyed-" + name;
                solutionInfo = ScoreManagerScript.instance.getOtherPlayersScore(playerIndex);
                do
                {
                    tempIndex = Random.Range(0, solutionInfo.data.Count);
                    row = solutionInfo.data[tempIndex].row;
                    column = solutionInfo.data[tempIndex].column;
                } while ((row == 0 && column == 0) || (row == 0 && column == 9) || (row == 9 && column == 0) || (row == 9 && column == 9));


                Debug.Log(tempIndex + "  " + row + "  " + column);
                return CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards.FindIndex(o => o.row == row && o.column == column);

                // PutCard(, true);
                // ScoreManagerScript.instance.ScoreOfCards[solutionInfo.data[Random.Range(0, solutionInfo.data.Count)].row, solutionInfo.data[Random.Range(0, solutionInfo.data.Count)].column] = -1;

                break;
            case "JackofClubs":
            case "JackofDiamonds":
                Debug.Log("2 eyed");
                JackCardName = "two eyed-" + name;
                solutionInfo = ScoreManagerScript.instance.getOtherPlayersScore(playerIndex);
                if (solutionInfo.length > 3)
                {
                    do
                    {
                        tempIndex = Random.Range(0, solutionInfo.data.Count);
                        row = solutionInfo.data[tempIndex].row;
                        column = solutionInfo.data[tempIndex].column;
                    } while ((row == 0 && column == 0) || (row == 0 && column == 9) || (row == 9 && column == 0) || (row == 9 && column == 9));


                    Debug.Log(tempIndex + "  " + row + "  " + column);
                    return CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards.FindIndex(o => o.row == row && o.column == column);
                    // PutCard(CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards.FindIndex(o => o.row == row && o.column == column), false);
                }
                else
                {

                    if ((index = ExtendSequenceByJack()) != -1)
                    {

                        Debug.Log(index);
                        return index;
                    }
                }
                break;
        }
        return -1;
    }
    int ExtendSequenceByJack()
    {
        Debug.Log("ExtendSequenceByJack");
        int nearestIndex = -1;
        int oldScore;

        MatrixOfCards matrixOfCards;
        for (int i = 0; i < CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards.Count; i++)
        {
            matrixOfCards = CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards[i];
            if (ScoreManagerScript.instance.ScoreOfCards.Find(o => o.row == matrixOfCards.row && o.column == matrixOfCards.column).value != playerIndex)
            {

                oldScore = ScoreManagerScript.instance.ScoreOfCards.Find(o => o.row == matrixOfCards.row && o.column == matrixOfCards.column).value;
                ScoreManagerScript.instance.ScoreOfCards.Find(o => o.row == matrixOfCards.row && o.column == matrixOfCards.column).value = playerIndex;

                if (Solution.longestLine(ScoreManagerScript.instance.ScoreOfCards, playerIndex).length == 4)
                {

                    // nearestIndex = MatrixOfOwnCards.FindIndex(o => o.row == matrixOfCards.row && o.column == matrixOfCards.column && o.deck == matrixOfCards.deck);
                    nearestIndex = i;
                }
                else if (Solution.longestLine(ScoreManagerScript.instance.ScoreOfCards, playerIndex).length == 3)
                {
                    // nearestIndex = MatrixOfOwnCards.FindIndex(o => o.row == matrixOfCards.row && o.column == matrixOfCards.column && o.deck == matrixOfCards.deck);
                    nearestIndex = i;
                }
                else if (Solution.longestLine(ScoreManagerScript.instance.ScoreOfCards, playerIndex).length == 2)
                {
                    // nearestIndex = MatrixOfOwnCards.FindIndex(o => o.row == matrixOfCards.row && o.column == matrixOfCards.column && o.deck == matrixOfCards.deck);
                    nearestIndex = i;
                }
                else
                {
                    nearestIndex = -1;
                }
                ScoreManagerScript.instance.ScoreOfCards.Find(o => o.row == matrixOfCards.row && o.column == matrixOfCards.column).value = oldScore;
            }
        }
        Debug.Log(nearestIndex);
        return nearestIndex;
    }
    bool checkIfPossible(int index)
    {
        if (ScoreManagerScript.instance.ScoreOfCards.Find(o => o.row == MatrixOfOwnCards[index].row && o.column == MatrixOfOwnCards[index].column).value == -1)
            return true;
        else
            return false;
    }

    public void PutCard(int index)
    {
        StartCoroutine(Utilities.Wait(1/*Random.Range(2, 10)*/, () =>
       {
           Debug.Log("PutCard");
           selectedChip = manageChip.getChip();

           Debug.Log(selectedChip);


           if (JackCardName != "")
           {
               Debug.Log("jack is here ");
               Debug.Log(" put card index " + JackCardIndex + " card " + MatrixOfOwnCards[JackCardIndex].Card.name + " row " + MatrixOfOwnCards[JackCardIndex].row + " column " + MatrixOfOwnCards[JackCardIndex].column);
               Debug.Log(" put card index " + index + " card " + CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards[index].Card.name + " row " + CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards[index].row + " column " + CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards[index].column);

               GameObject oldChip = CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards[index].Chip;
               if (oldChip != null)
                   Debug.Log(oldChip.name);

               Debug.Log(JackCardName.Split('-')[0]);
               if (JackCardName.Split('-')[0] == "one eyed")
               {
                   Debug.Log("destroy old chip");
                   Destroy(oldChip);

                   CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards[index].Chip = null;
                   ScoreManagerScript.instance.updateScore(CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards[index].Card, true);
               }
               if (JackCardName.Split('-')[0] == "two eyed")
               {
                   Debug.Log("destroy old chip and assign new chip");
                   if (oldChip != null)
                       Destroy(oldChip);
                   CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards[index].Chip = selectedChip;
                   PutChip = StartCoroutine(putChip(index));
                   MatrixOfUsedCards.Add(CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards[index]);
                   ScoreManagerScript.instance.updateScore(CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards[index].Card, false);
               }
               changeCard(MatrixOfOwnCards[JackCardIndex].Card);
               JackCardIndex = -1;
           }
           else
           {
               //    int i = CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards.FindIndex(o => o.Card.Equals(MatrixOfOwnCards[index].Card) && o.deck == MatrixOfOwnCards[index].deck);
               //    Debug.Log(i);

               //    //Debug.Log(" put card index " + index + " card " + MatrixOfOwnCards[index].Card + " row " + MatrixOfOwnCards[index].row + " column " + MatrixOfOwnCards[index].column);
               //    index = i;
               changeCard(CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards[index].Card);
               PutChip = StartCoroutine(putChip(index));
               CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards[index].Chip = selectedChip;
               MatrixOfUsedCards.Add(CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards[index]);
               ScoreManagerScript.instance.updateScore(CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards[index].Card, false);
           }




           //ScoreManagerScript.instance.ScoreOfCards[MatrixOfOwnCards[index].row, MatrixOfOwnCards[index].column] = playerIndex;

           GameManagerScript.instance.endRound();
       }));


    }
    public void changeCard(GameObject card)
    {
        Debug.Log("changeCard");
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
    IEnumerator putChip(int index)
    {
        AudioManager.instance.Play("Put Chip");
        Debug.Log("putChip");
        float elapsedTime = 0;
        float waitTime = 1f;
        Vector3 newPosition;

        newPosition = CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards[index].Card.transform.position;

        newPosition.y += 0.3f;
        while (elapsedTime < waitTime)
        {
            selectedChip.transform.position = Vector3.Lerp(selectedChip.transform.position, newPosition, Speed * Time.deltaTime);
            elapsedTime += Time.deltaTime;

            // Yield here
            yield return null;
        }
        // Make sure we got there
        selectedChip.transform.position = newPosition;

        yield return null;
    }
    public int BreakOtherPlayersPairs()
    {

        Debug.Log("BreakOtherPlayersPairs");
        int nearestIndex = -1;

        for (int i = 0; i < MatrixOfOwnCards.Count; i++)
        {
            if (MatrixOfOwnCards[i].row != 100)
            {
                Debug.Log(MatrixOfOwnCards[i].Card.name);
                Debug.Log(MatrixOfOwnCards[i].row);
                Debug.Log(MatrixOfOwnCards[i].column);
                int temp = ScoreManagerScript.instance.ScoreOfCards.Find(o => o.row == MatrixOfOwnCards[i].row && o.column == MatrixOfOwnCards[i].column).value;
                ScoreManagerScript.instance.ScoreOfCards.Find(o => o.row == MatrixOfOwnCards[i].row && o.column == MatrixOfOwnCards[i].column).value = playerIndex;

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
                ScoreManagerScript.instance.ScoreOfCards.Find(o => o.row == MatrixOfOwnCards[i].row && o.column == MatrixOfOwnCards[i].column).value = temp;
            }
        }
        return nearestIndex;
    }
    public int getNearestNeighbour()
    {

        Debug.Log("getNearestNeighbour");
        // int[] DataLength = new int[MatrixOfOwnCards.Length];
        int nearestIndex = -1;
        int minimumDistance = 100;

        // Dictionary<int, int> Data = new Dictionary<int, int>();
        for (int i = 0; i < MatrixOfOwnCards.Count; i++)
        {
            for (int j = 0; j < MatrixOfUsedCards.Count; j++)
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

        Debug.Log("nearToCorner");
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
            Play();
        }
        else
        {
            endGame();
        }


    }
    public void endGame()
    {
        Debug.Log("end game ai");

    }



    public void ShowCards()
    {

        ManageCard();

    }
    public void HideCards()
    {



    }

}

