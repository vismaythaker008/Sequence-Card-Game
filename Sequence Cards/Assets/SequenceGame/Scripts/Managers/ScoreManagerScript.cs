using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SequenceCardGame;


public class ScoreManagerScript : MonoBehaviour
{
    public static ScoreManagerScript instance;
    public Dictionary<int, int> SequenceInfo = new Dictionary<int, int>();
    public GameObject LinePrefab;

    // public int[,] ScoreOfCards;
    public List<ScoreInfo> ScoreOfCards;
    private SolutionInfo solutionInfo;

    void Awake()
    {

        instance = this;
        ScoreOfCards = new List<ScoreInfo>();
        // Debug.Log(ScoreOfCards.Length);
        for (int i = 0; i < 10; i++)
        {
            // ScoreOfCards[i] = new int[10];
            for (int j = 0; j < 10; j++)
            {

                ScoreInfo score = new ScoreInfo();
                score.row = i;
                score.column = j;
                if ((i == j && (i == 0 || i == 9)) || (i == 0 && j == 9) || (i == 9 && j == 0))
                {
                    score.value = -2;
                }
                else
                {
                    score.value = -1;
                }
                ScoreOfCards.Add(score);
                // Debug.Log("i,j=" + i + "," + j + " =" + ScoreOfCards[i, j]);
            }
        }
    }
    public void setUpScoreInfo(int PlayerCount)
    {
        for (int i = 0; i < PlayerCount; i++)
        {
            SequenceInfo.Add(i, 0);
        }
    }
    public void CheckScore()
    {
        for (int i = 0; i < GameManagerScript.instance.PlayerCount; i++)
        {
            solutionInfo = Solution.longestLine(ScoreOfCards, i);
            List<Vector3> positionsForLine = new List<Vector3>();
            Debug.Log("for player " + (i + 1));
            Debug.Log("length " + solutionInfo.length);
            Debug.Log("data count " + solutionInfo.data.Count);
            for (int j = 0; j < solutionInfo.data.Count; j++)
            {
                Debug.Log("row " + solutionInfo.data[j].row);
                Debug.Log("column " + solutionInfo.data[j].column);
                positionsForLine.Add((CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards.Find(o => o.row == solutionInfo.data[j].row && o.column == solutionInfo.data[j].column).Card.transform.position));
                Debug.Log(positionsForLine[j]);
            }
            if (solutionInfo.length >= 5)
            {
                LineRenderer lineRenderer = Instantiate(LinePrefab, new Vector3(0, 1, 0), Quaternion.identity, transform).GetComponent<LineRenderer>();
                lineRenderer.positionCount = 5;
                for (int p = 0; p < 5; p++)
                {
                    lineRenderer.SetPosition(p, positionsForLine[p]);
                }
                // lineRenderer.SetPositions(positionsForLine.GetRange(0, 5).ToArray());
                SequenceInfo[GameManagerScript.instance.curentPlayerIndex]++;

            }
            else if (solutionInfo.length == 5)
            {
                LineRenderer lineRenderer = Instantiate(LinePrefab, new Vector3(0, 1, 0), Quaternion.identity, transform).GetComponent<LineRenderer>();
                lineRenderer.positionCount = 5;
                for (int p = 0; p < positionsForLine.Count; p++)
                {
                    lineRenderer.SetPosition(p, positionsForLine[p]);
                }

                // lineRenderer.SetPositions(positionsForLine.ToArray());
                SequenceInfo[GameManagerScript.instance.curentPlayerIndex]++;
            }
            if (SequenceInfo[GameManagerScript.instance.curentPlayerIndex] == 2)
            {
                Debug.Log("player with index " + GameManagerScript.instance.curentPlayerIndex + " wins");
            }

        }
    }
    public SolutionInfo getOtherPlayersScore(int index)
    {
        SolutionInfo maxSolution = new SolutionInfo();
        for (int i = 0; i < GameManagerScript.instance.PlayerCount; i++)
        {
            if (i != index)
            {
                solutionInfo = Solution.longestLine(ScoreOfCards, i);
                if (solutionInfo.length > maxSolution.length)
                {
                    maxSolution = solutionInfo;
                }

            }

        }
        return maxSolution;

    }
    public void updateScore(GameObject Card, bool jack)
    {
        int value;
        if (jack)
        {
            value = -1;
        }
        else
        {
            value = GameManagerScript.instance.curentPlayerIndex;
            Debug.Log(value);
        }

        MatrixOfCards grid = CardsManagerScript.instance.ListOfGridMatrixOfTotalDisplayCards.Find(x => x.Card.Equals(Card));
        Debug.Log("Score Update Start");
        Debug.Log("row :" + grid.row + " column: " + grid.column + " card: " + grid.Card.name + " value " + value + " Score " + ScoreOfCards.Find(p => p.row == grid.row && p.column == grid.column).value);
        ScoreOfCards.Find(p => p.row == grid.row && p.column == grid.column).value = value;
        Debug.Log("row :" + grid.row + " column: " + grid.column + " card: " + grid.Card.name + " value " + value + " Score " + ScoreOfCards.Find(p => p.row == grid.row && p.column == grid.column).value);
        Debug.Log("Score Update end");
    }



}
[System.Serializable]
public class ScoreInfo
{
    public int row;
    public int column;
    public int value;


}