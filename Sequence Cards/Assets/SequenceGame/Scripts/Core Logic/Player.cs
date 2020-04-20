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
    private Material material;
    private Coroutine RaycastToCards;
    private string selectedCardName = null;
    public int CardCount = 0;
    public int TotalCardCount;
    public int playerIndex;
    public string chipTag;


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
        Debug.Log("outside while");
        Debug.Log(TotalCardCount);
        while (CardCount < TotalCardCount)
        {
            Debug.Log("inside while");
            GameObject Card = CardsManagerScript.instance.getCard();
            GameObject c = Instantiate(Card, CardPositions[CardCount]);
            c.name = Card.name;
            c.transform.localScale = Vector3.one * 100f;
            c.tag = ConstantString.TagForPlayingCards;
            Debug.Log(CardCount);
            Cards.Add(CardCount, c);
            CardCount++;
        }
        yield return null;
    }
    public void changeCard(GameObject card)
    {
        int cardPositionIndex = Cards.KeyByValue(card);
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
            if (hit.collider.CompareTag(chipTag))
            {
                Debug.Log(selectedChip);
                if (selectedChip == null)
                {
                    selectedChip = hit.collider.gameObject;
                    selectedChip.GetComponent<Chip>().putChip = true;
                }
            }
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
        if (selectedChip != null)
            selectedChip.GetComponent<Chip>().putChip = false;
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
