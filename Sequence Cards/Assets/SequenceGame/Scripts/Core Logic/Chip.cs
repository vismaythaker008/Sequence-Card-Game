using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SequenceCardGame;

public class Chip : MonoBehaviour
{
    public bool putChip = false;
    public bool setChip = false;
    private Dictionary<int, GameObject> playerCards;
    private Player player;
    private Vector3 initialPosition = Vector3.zero;

    // void Start()
    // {
    //     // initialPosition = new Vector3(transform.position.x, -4f, transform.position.z);
    //     //GameStateManager.onGameStateChange += OnGameStateChange;
    //     // SwipeDetection.initialPosition += IntialPosition;
    //     // SwipeDetection.finalPosition += FinalPosition;
    // }
    void OnCollisionEnter(Collision other)
    {
        if (initialPosition == Vector3.zero)
        {
            initialPosition = transform.position;
            player = GameObject.Find("Player").GetComponent<Player>();
            playerCards = player.Cards;

        }
    }
    private void OnEnable()
    {
        // Debug.Log("game satate event sucscribed");
        GameStateManager.onGameStateChange += OnGameStateChange;

    }

    private void OnDisable()
    {
        // Debug.Log("game satate event unsucscribed");
        GameStateManager.onGameStateChange -= OnGameStateChange;
    }
    void OnGameStateChange(GameState currentGameState)
    {

        if (currentGameState == GameState.GamePlay)
        {

            // Debug.Log("player cards count " + playerCards.Count);
            // Debug.Log("called"); 
            SwipeDetection.initialPosition += IntialPosition;
            SwipeDetection.finalPosition += FinalPosition;

        }
        else
        {
            SwipeDetection.initialPosition += IntialPosition;
            SwipeDetection.finalPosition -= FinalPosition;
        }
        //SwipeDetection.CurveSwipe += CurveSwipeForceDirection;
        //test.Swipes += SwipeForceDirection;
        // change after completion of curve to swipe dectection

    }

    public void IntialPosition(Vector3 Position)
    {
        if (putChip && !setChip)
        {
            float planeY = 0;

            Plane plane = new Plane(Vector3.up, Vector3.up * planeY); // ground plane

            Ray ray = Camera.main.ScreenPointToRay(Position);
            // Debug.Log("init");
            float distance; // the distance from the ray origin to the ray intersection of the plane
            if (plane.Raycast(ray, out distance))
            {
                transform.position = new Vector3(ray.GetPoint(distance).x, transform.position.y, ray.GetPoint(distance).z); // distance along the ray
                transform.localPosition = new Vector3(transform.localPosition.x, -3.2f, transform.localPosition.z);
            }

        }


    }
    public void FinalPosition()
    {
        if (putChip && !setChip)
        {
            RaycastHit hit;
            // Debug.Log("final");
            Vector3 fwd = transform.TransformDirection(Vector3.down);
            Debug.DrawRay(transform.position, fwd * 50, Color.green);
            if (Physics.Raycast(transform.position, fwd, out hit, 50))
            {
                Debug.Log(hit.collider.name);
                if (hit.collider.CompareTag(ConstantString.TagForDisplayCards) && checkIfCardExist(hit.collider.name))
                {
                    Debug.Log(hit.collider.transform.position);
                    Debug.Log("inside if");
                    // Vector3 position =  Camera.main.ScreenToWorldPoint(hit.collider.transform.position);
                    transform.position = new Vector3(hit.collider.transform.position.x, transform.position.y, hit.collider.transform.position.z);
                    transform.localPosition = new Vector3(transform.localPosition.x, -3.2f, transform.localPosition.z);
                    GetComponentInParent<ManageChip>().chipCount--;
                    setChip = true;
                    player.selectedChip = null;
                    player.changeCard(hit.collider.gameObject);
                    GameManagerScript.instance.endRound();
                    player.CardsGlow("all", true);
                }
                else
                {
                    transform.position = initialPosition;
                }
            }




        }


    }
    bool checkIfCardExist(string name)
    {
        foreach (var item in playerCards)
        {
            if (item.Value.name == name)
            {
                return true;
            }

        }
        return false;
    }
}