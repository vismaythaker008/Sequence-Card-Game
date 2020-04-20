using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SequenceCardGame;
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;
    GameState currentGameState = GameState.None;

    public delegate void OnGameStateChange(GameState gameState);
    public static event OnGameStateChange onGameStateChange;

    private void Awake()
    {
        Instance = this;

    }
    public void ChangeGameState(GameState gameState)
    {

        if (currentGameState != gameState)
        {
            currentGameState = gameState;
            if (onGameStateChange != null)
            {
                onGameStateChange(currentGameState);
            }
        }

    }

    public bool CheckGameState(GameState gameState)
    {
        return currentGameState == gameState;
    }

}

