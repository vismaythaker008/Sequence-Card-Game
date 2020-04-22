using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SequenceCardGame;
using cakeslice;
using System.Linq;
using System;

public class Player : MonoBehaviour
{
    public Transform[] CardPositions;
    public Dictionary<int, GameObject> Cards = new Dictionary<int, GameObject>();
    private Coroutine Play;
    private Coroutine ManageCards;
    public GameObject selectedChip = null;
    public List<MatrixOfCards> MatrixOfOwnCards;
    private Material material;
    private Coroutine RaycastToCards;
    private string selectedCardName = null;
    public int CardCount = 0;
    public int TotalCardCount;
    public int playerIndex;
    public List<MatrixOfCards> GridMatrixOfTotalDisplayCards;
    public string chipTag;


    void OnEnable()
    {
        GridMatrixOfTotalDisplayCards = new List<MatrixOfCards>(CardsManagerScript.instance.GridMatrixOfTotalDisplayCards);
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

                MatrixOfOwnCards.Add(GridMatrixOfTotalDisplayCards.Find(x => x.Card.name == Card.name));


                MatrixOfOwnCards.Add(GridMatrixOfTotalDisplayCards.FindLast(x => x.Card.name == Card.name));


            }
            CardCount++;
        }
        yield return null;
    }
    public bool checkIfCardExist(GameObject card)
    {
        // foreach (var item in GridMatrixOfTotalDisplayCards)
        // {
        //     Debug.Log("name " + card.name == item.Card.name);
        //     Debug.Log("data " + card.Equals(item.Card));
        // }

        MatrixOfCards grid = GridMatrixOfTotalDisplayCards.Find(x => x.Card.Equals(card));

        Debug.Log(grid.row);
        Debug.Log(grid.column);
        Debug.Log(grid.Card.name);
        Debug.Log(MatrixOfOwnCards.Contains(grid));
        Debug.Log(ScoreManagerScript.instance.ScoreOfCards[grid.row, grid.column]);
        if (MatrixOfOwnCards.Contains(grid) && ScoreManagerScript.instance.ScoreOfCards[grid.row, grid.column] == -1)
            return true;
        else
            return false;

    }
    public void changeCard(GameObject card)
    {
        int cardPositionIndex = -1;
        foreach (var keyvaluepair in Cards)
        {
            if (keyvaluepair.Value.name == card.name)
            {
                //dict.Remove(keyvaluepair.Key);
                cardPositionIndex = keyvaluepair.Key;
                break;
            }
        }


        Debug.Log(cardPositionIndex);
        int cardPositionIndexInMatrix;
        Cards.TryGetValue(cardPositionIndex, out GameObject oldCard);
        Destroy(oldCard);
        Cards.Remove(cardPositionIndex);

        Debug.Log("change card");
        GameObject Card = CardsManagerScript.instance.getCard();
        GameObject c = Instantiate(Card, CardPositions[cardPositionIndex]);
        c.name = Card.name;
        c.transform.localScale = Vector3.one * 100f;
        c.tag = ConstantString.TagForPlayingCards;
        Cards.Add(cardPositionIndex, c);
        if (Card.name.Contains("Jack"))
        {
            cardPositionIndexInMatrix = MatrixOfOwnCards.FindIndex(o => o.Card.name == card.name);
            MatrixOfOwnCards.Remove(MatrixOfOwnCards[cardPositionIndexInMatrix]);
            cardPositionIndexInMatrix = MatrixOfOwnCards.FindLastIndex(o => o.Card.name == card.name);
            MatrixOfOwnCards.Remove(MatrixOfOwnCards[cardPositionIndexInMatrix]);
            MatrixOfCards matrixOfCard = new MatrixOfCards();
            matrixOfCard.row = 100;
            matrixOfCard.column = 100;
            matrixOfCard.Card = Card;
            MatrixOfOwnCards.Add(matrixOfCard);
        }
        else
        {
            cardPositionIndexInMatrix = MatrixOfOwnCards.FindIndex(o => o.Card.name == card.name);
            MatrixOfOwnCards.Remove(MatrixOfOwnCards[cardPositionIndexInMatrix]);

            MatrixOfOwnCards.Add(GridMatrixOfTotalDisplayCards.Find(x => x.Card.name == Card.name));

            MatrixOfOwnCards.Add(GridMatrixOfTotalDisplayCards.FindLast(x => x.Card.name == Card.name));
            // MatrixOfOwnCards.Add(GridMatrixOfTotalDisplayCards.Find(x => x.Card.name == Card.name));
            int tempIndex = MatrixOfOwnCards.FindIndex(o => o.Card.name == Card.name);
            Debug.Log(" index " + tempIndex + " card " + MatrixOfOwnCards[tempIndex].Card + " row " + MatrixOfOwnCards[tempIndex].row + " column " + MatrixOfOwnCards[tempIndex].column);

            // MatrixOfOwnCards.Add(GridMatrixOfTotalDisplayCards.FindLast(x => x.Card.name == Card.name));
            tempIndex = MatrixOfOwnCards.FindLastIndex(o => o.Card.name == Card.name);
            Debug.Log(" index " + tempIndex + " card " + MatrixOfOwnCards[tempIndex].Card + " row " + MatrixOfOwnCards[tempIndex].row + " column " + MatrixOfOwnCards[tempIndex].column);

        }
    }
    IEnumerator RaycastForCards()
    {
        while (true)
        {
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
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out hit))
        {
            // Debug.Log("throw ray");
            if (hit.collider.CompareTag(ConstantString.TagForPlayingCards))
            {
                if (selectedCardName != null)
                {
                    CardsGlow(selectedCardName, true);
                }
                selectedCardName = hit.collider.name;
                CardsGlow(selectedCardName, false);

                // Debug.Log(card.GetComponent<MeshRenderer>().material.GetFloat("_SmoothnessTextureChannel"));
                // material = card.GetComponent<MeshRenderer>().material;
                // material.SetFloat("_SmoothnessTextureChannel", 0.5f);
            }
            else if (selectedChip == null && hit.collider.CompareTag(chipTag))
            {
                Debug.Log("assigned");
                selectedChip = hit.collider.gameObject;
                selectedChip.GetComponent<Chip>().putChip = true;
                //selectedChip.GetComponent<Chip>().setChip = false;

            }
            // Debug.Log(selectedChip);
        }
    }
    public void CardsGlow(string name, bool glow)
    {
        IEnumerable<GameObject> cards;
        if (name != "all")
        {
            cards = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == name);
            // Debug.Log(name + cards);
            foreach (GameObject item in cards)
            {
                item.transform.GetComponent<Outline>().eraseRenderer = glow;
            }
        }
        else
        {
            cards = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.CompareTag(ConstantString.TagForDisplayCards));
            foreach (GameObject item in cards)
            {
                item.transform.GetComponent<Outline>().eraseRenderer = glow;
            }
            cards = (Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.CompareTag(ConstantString.TagForPlayingCards)));
            foreach (GameObject item in cards)
            {
                item.transform.GetComponent<Outline>().eraseRenderer = glow;
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
    IEnumerator play()
    {

        yield return null;
    }
    public void startGame(int index)
    {
        if (playerIndex == index)
        {
            Debug.Log("start game Player");
            chipTag = GetComponentInChildren<ManageChip>().getchipTag();
            Play = StartCoroutine(play());
            RaycastToCards = StartCoroutine(RaycastForCards());
        }
        else
        {
            endGame();
        }
    }
    public void endGame()
    {
        Debug.Log("end game Player");
        if (Play != null)
            StopCoroutine(Play);
        // if (selectedChip != null)
        //     selectedChip.GetComponent<Chip>().putChip = false;
        if (RaycastToCards != null)
            StopCoroutine(RaycastToCards);
    }
    public void ShowCards()
    {
        Debug.Log("player show cards");
        ManageCards = StartCoroutine(ManageCard());
    }
    public void HideCards()
    {
        StopCoroutine(ManageCards);

    }
}
