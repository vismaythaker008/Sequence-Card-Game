using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SequenceCardGame;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class SwipeDetection : MonoBehaviour
{
    Vector3 Position;

    public delegate void InitialPosition(Vector3 startPos);
    public static InitialPosition initialPosition;
    public delegate void FinalPosition();
    public static FinalPosition finalPosition;




    private void OnEnable()
    {
        // GameStateManager.onGameStateChange += OnGameStateChange;

    }
    private void OnDisable()
    {
        // GameStateManager.onGameStateChange -= OnGameStateChange;

    }

    void Update()
    {

#if UNITY_EDITOR


        MouseInputs();

#else

        MobileTouchInputs();


#endif


    }
    void MouseInputs()
    {

        if (Input.GetMouseButton(0))
        {
            if (initialPosition != null)
            {
                initialPosition(Input.mousePosition);
                // Debug.Log("called");
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (finalPosition != null)
            {
                finalPosition();
                // Debug.Log("called");
            }
        }


    }


    void MobileTouchInputs()
    {

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Position = Input.GetTouch(0).position;
            if (initialPosition != null)
                initialPosition(Position);
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Position = Input.GetTouch(0).position;
            if (initialPosition != null)
                initialPosition(Position);


        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            Position = Input.GetTouch(0).position;
            if (finalPosition != null)
                finalPosition();


        }

    }



}
