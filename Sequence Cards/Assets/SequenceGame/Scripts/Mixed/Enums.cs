using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SequenceCardGame
{

    public enum GameState
    {
        None,
        MainMenu,

        GamePlay,
        PausedGamePlay,
        GameOver,
    }
    public enum BallState
    {

        None,
        BallIdle,
        BallMoving,
        BallPaused,
        BallMissed,
        BallHit
    }
    public enum SceneName
    {
        None,
        MainMenu,
        SelectionMenu,
        PlayerSelectionMenu,
        GamePlay,
        PausedGamePlay,
        GameOver,
        SettingsMenu,
        Instructions,
    }

    public enum GameMode
    {

        None,
        LearnToPlay,
        PlayerVsAI,
        OnlineMultiplayer
    }
}