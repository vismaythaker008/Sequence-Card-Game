using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SequenceCardGame;
using cakeslice;
using System.Linq;
using System;

public class Player : MonoBehaviour
{
    #region variables
    public Transform[] CardPositions;
    public float Speed = 9f;
    public Dictionary<int, GameObject> Cards = new Dictionary<int, GameObject>();
    public List<MatrixOfCards> PossibleCardsForJacks = new List<MatrixOfCards>();


    public GameObject selectedChip = null;
    public List<MatrixOfCards> MatrixOfOwnCards;

    private ManageChip manageChip;
    private Coroutine RaycastToCards;
    private string selectedCardName = null;
    public int CardCount = 0;
    public int TotalCardCount;
    public int playerIndex;

    public string chipTag;
    int layer_mask;
    public String JackCardName = "";
    private Coroutine PutChip;


    #endregion
    void OnEnable()
    {

        CardsManagerScript.OnTotalCardFound += setTotalCardCount;
        GameManagerScript.TurnChanged += startGame;
    }
    void Start()
    {
        layer_mask = LayerMask.GetMask("Cards");
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
    void ManageCards()
    {
        Debug.Log("ManageCard");
        // Debug.Log(TotalCardCount);
        while (CardCount < TotalCardCount)
        {

            GameObject Card = CardsManagerScript.instance.getCard();
            GameObject c = Instantiate(Card, CardPositions[CardCount]);
            c.name = Card.name;
            c.transform.localScale = Vector3.one * 100f;
            c.tag = ConstantString.TagForPlayingCards;
            // Debug.Log("card count" + CardCount + " and Card " + c);
            Cards.Add(CardCount, c);
            // Debug.Log("card count" + CardCount + " and Card  after " + Cards[CardCount]);
            if (Card.name.Contains("Jack"))
            {
                MatrixOfCards matrixOfCards = new MatrixOfCards();
                matrixOfCards.row = 100;
                matrixOfCards.column = 100;
                matrixOfCards.Card = Card;
                MatrixOfOwnCards.Add(matrixOfCards);

            }
            else
            {

                MatrixOfOwnCards.Add(CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards.Find(x => x.Card.name == Card.name));
                MatrixOfOwnCards.Add(CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards.FindLast(x => x.Card.name == Card.name));
            }
            CardCount++;
        }

    }
    public bool checkIfCardExist(GameObject card)
    {
        Debug.Log("checkIfCardExist");
        // foreach (var item in GridMatrixOfTotalDisplayCards)
        // {
        //     Debug.Log("name " + card.name == item.Card.name);
        //     Debug.Log("data " + card.Equals(item.Card));
        // }
        if (PossibleCardsForJacks.Count != 0)
        {
            MatrixOfCards grid = CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards.Find(x => x.Card.Equals(card));

            if (PossibleCardsForJacks.Contains(grid))
            {
                return true;
            }
            else
                return false;
        }
        else
        {
            MatrixOfCards grid = CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards.Find(x => x.Card.Equals(card));

            Debug.Log(grid.row);
            Debug.Log(grid.column);
            Debug.Log(grid.Card.name);
            Debug.Log(MatrixOfOwnCards.Contains(grid));
            Debug.Log(ScoreManagerScript.instance.ScoreOfCards.Find(o => o.row == grid.row && o.column == grid.column).value);
            if (MatrixOfOwnCards.Contains(grid) && ScoreManagerScript.instance.ScoreOfCards.Find(o => o.row == grid.row && o.column == grid.column).value == -1)
                return true;
            else
                return false;
        }


    }
    IEnumerator putChip(GameObject Card)
    {
        AudioManager.instance.Play("Put Chip");
        Debug.Log("putChip");
        float elapsedTime = 0;
        float waitTime = 1f;
        Vector3 newPosition = Card.transform.position;

        newPosition.y += 0.3f;
        while (elapsedTime < waitTime)
        {
            selectedChip.transform.position = Vector3.Lerp(selectedChip.transform.position, newPosition, Speed * Time.deltaTime);
            elapsedTime += Time.deltaTime;

            // Yield here
            yield return new WaitForEndOfFrame();
        }
        // Make sure we got there
        selectedChip.transform.position = newPosition;


        yield return null;
    }
    public void changeCard(GameObject card)
    {
        Debug.Log("changeCard");
        // AudioManager.instance.Play("Change Card");
        string name;
        int cardPositionIndex = -1;
        if (JackCardName != "")
        {
            name = JackCardName.Split('-')[1];
            Debug.Log(name);
            MakeCardValueAssignedDueToJack(card);
        }
        else
        {
            name = card.name;
            Debug.Log(name);
        }

        foreach (KeyValuePair<int, GameObject> keyvaluepair in Cards)
        {
            Debug.Log(keyvaluepair.Key);
            Debug.Log(keyvaluepair.Value.name);
            if (keyvaluepair.Value.name == name)
            {
                // dict.Remove(keyvaluepair.Key);
                cardPositionIndex = keyvaluepair.Key;
                Debug.Log(cardPositionIndex);
                break;
            }
        }
        Debug.Log(cardPositionIndex);
        int cardPositionIndexInMatrix;
        Cards.TryGetValue(cardPositionIndex, out GameObject oldCard);
        if (cardPositionIndex != -1)
        {
            Debug.Log(oldCard.name);
            Destroy(oldCard);
            Debug.Log(Cards.Remove(cardPositionIndex));
        }

        Debug.Log("change card");
        GameObject Card = CardsManagerScript.instance.getCard();
        Debug.Log("new card " + Card.name);
        GameObject c = Instantiate(Card, CardPositions[cardPositionIndex]);
        c.name = Card.name;
        c.transform.localScale = Vector3.one * 100f;
        c.tag = ConstantString.TagForPlayingCards;
        Cards.Add(cardPositionIndex, c);
        if (JackCardName != "")
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
                int deckCount = 1;
                MatrixOfOwnCards.Add(CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards.Find(x => x.Card.name == Card.name && x.deck == deckCount));
                deckCount++;
                MatrixOfOwnCards.Add(CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards.Find(x => x.Card.name == Card.name && x.deck == deckCount));

                // MatrixOfOwnCards.Add(GridMatrixOfTotalDisplayCards.FindLast(x => x.Card.name == Card.name));


                // MatrixOfOwnCards.Add(GridMatrixOfTotalDisplayCards.Find(x => x.Card.name == Card.name));
                int tempIndex = MatrixOfOwnCards.FindIndex(o => o.Card.name == Card.name && o.deck == 1);
                Debug.Log(" index " + tempIndex + " card " + MatrixOfOwnCards[tempIndex].Card + " row " + MatrixOfOwnCards[tempIndex].row + " column " + MatrixOfOwnCards[tempIndex].column);

                // MatrixOfOwnCards.Add(GridMatrixOfTotalDisplayCards.FindLast(x => x.Card.name == Card.name));
                tempIndex = MatrixOfOwnCards.FindLastIndex(o => o.Card.name == Card.name && o.deck == 2);
                Debug.Log(" index " + tempIndex + " card " + MatrixOfOwnCards[tempIndex].Card + " row " + MatrixOfOwnCards[tempIndex].row + " column " + MatrixOfOwnCards[tempIndex].column);

            }
            JackCardName = "";
        }
        else
        {
            cardPositionIndexInMatrix = MatrixOfOwnCards.FindIndex(o => o.Card.name == name && o.deck == 1);
            MatrixOfOwnCards.Remove(MatrixOfOwnCards[cardPositionIndexInMatrix]);
            cardPositionIndexInMatrix = MatrixOfOwnCards.FindIndex(o => o.Card.name == name && o.deck == 2);
            // cardPositionIndexInMatrix = MatrixOfOwnCards.FindLastIndex(o => o.Card.name == name);
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
                tempIndex = MatrixOfOwnCards.FindLastIndex(o => o.Card.name == Card.name && o.deck == 2);
                Debug.Log(" index " + tempIndex + " card " + MatrixOfOwnCards[tempIndex].Card + " row " + MatrixOfOwnCards[tempIndex].row + " column " + MatrixOfOwnCards[tempIndex].column);

            }
        }

    }
    void MakeCardValueAssignedDueToJack(GameObject Card)
    {
        MatrixOfCards matrixOfCard = CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards.Find(c => c.Card.Equals(Card));
        CardsManagerScript.instance.TotalCards.Find(c => c.Card.name == Card.name && matrixOfCard.deck == c.deck).assign();
    }
    IEnumerator RaycastForCards()
    {
        while (true)
        {
            // Debug.Log("RaycastForCards");
#if UNITY_EDITOR


            MouseInputs();

#else

        MobileTouchInputs();


#endif
            yield return null;

        }

    }
    void MouseInputs()
    {

        if (Input.GetMouseButton(0))
        {
            ThrowRay(Input.mousePosition);
        }


    }

    void ThrowRay(Vector3 position)
    {
        // Debug.Log("ThrowRay");
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out hit, 50, layer_mask))
        {
            Debug.Log(hit.collider.name);
            if (hit.collider.CompareTag(ConstantString.TagForPlayingCards))
            {
                if (selectedCardName != null)
                {
                    CardsGlow(selectedCardName, true);
                }
                selectedCardName = hit.collider.name;
                // Debug.Log(selectedCardName);
                CardsGlow(selectedCardName, false);

                // Debug.Log(card.GetComponent<MeshRenderer>().material.GetFloat("_SmoothnessTextureChannel"));
                // material = card.GetComponent<MeshRenderer>().material;
                // material.SetFloat("_SmoothnessTextureChannel", 0.5f);
            }
            else if (hit.collider.CompareTag(ConstantString.TagForDisplayCards) && checkIfCardExist(hit.collider.gameObject))
            {

                selectedChip = manageChip.getChip();

                Debug.Log(selectedChip);
                Debug.Log(hit.collider.name);
                Debug.Log(JackCardName);
                int index = CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards.FindIndex(o => o.Card.transform == hit.collider.transform);
                Debug.Log(index);
                if (JackCardName != "")
                {
                    Debug.Log("jack is here ");
                    GameObject oldChip = CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards[index].Chip;
                    if (oldChip != null)
                        Debug.Log(oldChip.name);

                    Debug.Log(JackCardName.Split('-')[0]);
                    if (JackCardName.Split('-')[0] == "one eyed")
                    {
                        Debug.Log("destroy old chip");
                        Destroy(oldChip);
                        ScoreManagerScript.instance.updateScore(hit.collider.gameObject, true);
                        CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards[index].Chip = null;
                    }
                    if (JackCardName.Split('-')[0] == "two eyed")
                    {
                        Debug.Log("destroy old chip and assign new chip");
                        if (oldChip != null)
                            Destroy(oldChip);
                        CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards[index].Chip = selectedChip;
                        PutChip = StartCoroutine(putChip(hit.collider.gameObject));
                        ScoreManagerScript.instance.updateScore(hit.collider.gameObject, false);
                    }
                }
                else
                {
                    PutChip = StartCoroutine(putChip(hit.collider.gameObject));
                    CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards[index].Chip = selectedChip;
                    ScoreManagerScript.instance.updateScore(hit.collider.gameObject, false);
                }
                changeCard(hit.collider.gameObject);

                GameManagerScript.instance.endRound();
                CardsGlow("all", true);

                // else if (selectedChip == null && hit.collider.CompareTag(chipTag))
                // {
                //     Debug.Log("assigned");
                //     selectedChip = hit.collider.gameObject;
                //     selectedChip.GetComponent<Chip>().putChip = true;
                //     //selectedChip.GetComponent<Chip>().setChip = false;

                // }

                // Debug.Log(selectedChip);
            }

        }
    }
    public void CardsGlow(string name, bool glow)
    {
        Debug.Log("CardsGlow");
        IEnumerable<GameObject> cards;


        if (name == "JackofSpades" || name == "JackofHearts")
        {
            JackCardName = "one eyed-" + name;
            //one eyed jack
            Debug.Log("one eyed jack");
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if ((i == 0 && j == 0) || (i == 0 && j == 9) || (i == 9 && j == 0) || (i == 9 && j == 9))
                    { }
                    else
                    {
                        int value = ScoreManagerScript.instance.ScoreOfCards.Find(o => o.row == i && o.column == j).value;
                        Debug.Log(value);
                        if (value != playerIndex && value != -1)
                        {
                            MatrixOfCards matrixOfCard = CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards.Find(o => o.row == i && o.column == j);
                            if (!PossibleCardsForJacks.Contains(matrixOfCard))
                                PossibleCardsForJacks.Add(matrixOfCard);
                            matrixOfCard.Card.transform.GetComponent<Outline>().eraseRenderer = glow;
                        }
                    }
                }
            }
            Debug.Log(PossibleCardsForJacks.Count);
        }
        else if (name == "JackofClubs" || name == "JackofDiamonds")
        {
            JackCardName = "two eyed-" + name;
            //two eyed jack
            Debug.Log("two eyed jack");
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if ((i == 0 && j == 0) || (i == 0 && j == 9) || (i == 9 && j == 0) || (i == 9 && j == 9) || ScoreManagerScript.instance.ScoreOfCards.Find(o => o.row == i && o.column == j).value == playerIndex)
                    { }
                    else
                    {
                        MatrixOfCards matrixOfCard = CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards.Find(o => o.row == i && o.column == j);
                        Debug.Log(matrixOfCard.Card.name);
                        if (!PossibleCardsForJacks.Contains(matrixOfCard))
                            PossibleCardsForJacks.Add(matrixOfCard);
                        matrixOfCard.Card.transform.GetComponent<Outline>().eraseRenderer = glow;

                    }
                }
            }
            Debug.Log(PossibleCardsForJacks.Count);
        }
        else if (name != "all")
        {
            List<MatrixOfCards> matrixOfCards = new List<MatrixOfCards>();

            JackCardName = "";
            PossibleCardsForJacks.Clear();
            Debug.Log(name);
            cards = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == name);
            matrixOfCards = CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards.FindAll(o => o.Card.name == name);
            // Debug.Log(name + cards);

            foreach (GameObject item in cards)
            {
                Debug.Log(item.name);
                if (item.GetComponent<Outline>() != null)
                    item.GetComponent<Outline>().eraseRenderer = glow;
            }

        }
        else
        {
            JackCardName = "";
            PossibleCardsForJacks.Clear();
            cards = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.CompareTag(ConstantString.TagForDisplayCards));
            foreach (GameObject item in cards)
            {
                item.GetComponent<Outline>().eraseRenderer = glow;
            }
            cards = (Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.CompareTag(ConstantString.TagForPlayingCards)));
            foreach (GameObject item in cards)
            {
                item.GetComponent<Outline>().eraseRenderer = glow;
            }

        }

        // GameObject[] cards = FindGameObjectsWithName(hit.collider.gameObject.name);
        // Debug.Log(cards.Count());

    }
    void MobileTouchInputs()
    {

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            ThrowRay(Input.GetTouch(0).position);
        }


    }

    public void startGame(int index)
    {
        if (playerIndex == index)
        {
            Debug.Log("start game Player");
            manageChip = transform.GetComponentInChildren<ManageChip>();
            chipTag = GetComponentInChildren<ManageChip>().getchipTag();

            RaycastToCards = StartCoroutine("RaycastForCards");
        }
        else
        {
            endGame();
        }
    }
    public void endGame()
    {
        Debug.Log("end game Player");

        // if (selectedChip != null)
        //     selectedChip.GetComponent<Chip>().putChip = false;
        // if (RaycastToCards != null)
        //StopCoroutine(RaycastToCards);
        StopCoroutine("RaycastForCards");
    }
    public void ShowCards()
    {
        Debug.Log("player show cards");
        ManageCards();
    }
    public void HideCards()
    {


    }
}
