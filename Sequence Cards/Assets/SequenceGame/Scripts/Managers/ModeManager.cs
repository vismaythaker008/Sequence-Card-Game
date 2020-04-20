using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SequenceCardGame;

[CreateAssetMenu(menuName = "ModeManager")]
public class ModeManager : ScriptableObject
{

    public delegate void OnModeChange(GameMode mode);
    public event OnModeChange ModeChanged;
    Mode currentMode = null;
    public List<Mode> Mode;

    public void ChangeGameMode(Mode Mode)
    {

        if (currentMode != null)
        {
            currentMode.OnExit();
        }
        currentMode = Mode;
        currentMode.OnEnter();
        ModeChanged(currentMode.gameMode);
    }
    public GameMode GetCurrentMode()
    {
        return currentMode.gameMode;
    }
}
