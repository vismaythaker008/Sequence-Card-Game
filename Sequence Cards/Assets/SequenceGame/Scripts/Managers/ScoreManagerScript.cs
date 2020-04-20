using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SequenceCardGame;


public class ScoreManagerScript : MonoBehaviour
{
    public static ScoreManagerScript instance;
    public int[,] ScoreOfCards;
    public int[,] indexOfSequencedCards;
    private SolutionInfo solutionInfo;

    void start()
    {
        instance = this;
        ScoreOfCards = new int[10, 10];
        // Debug.Log(ScoreOfCards.Length);
        for (int i = 0; i < 10; i++)
        {
            // ScoreOfCards[i] = new int[10];
            for (int j = 0; j < 10; j++)
            {
                ScoreOfCards[i, j] = -1;
                // Debug.Log("i,j=" + i + "," + j + " =" + ScoreOfCards[i, j]);
            }
        }
        ScoreOfCards[0, 0] = -2;
        ScoreOfCards[9, 0] = -2;
        ScoreOfCards[0, 9] = -2;
        ScoreOfCards[9, 9] = -2;
    }
    public void CheckScore()
    {
        for (int i = 0; i < GameManagerScript.instance.PlayerCount; i++)
        {
            solutionInfo = Solution.longestLine(ScoreOfCards, i);

            Debug.Log("for player " + i + 1);
            Debug.Log("length " + solutionInfo.length);
            for (int j = 0; j < solutionInfo.data.Count; j++)
            {
                Debug.Log("row " + solutionInfo.data[j].row);
                Debug.Log("row " + solutionInfo.data[j].column);
            }

        }
    }



}
