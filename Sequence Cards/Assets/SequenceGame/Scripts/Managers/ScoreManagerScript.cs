using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SequenceCardGame;


public class ScoreManagerScript : MonoBehaviour
{
    public static ScoreManagerScript instance;
    public GameObject LinePrefab;
    public List<MatrixOfCards> GridMatrixOfTotalDisplayCards;
    public int[,] ScoreOfCards;
    public int[,] indexOfSequencedCards;
    private SolutionInfo solutionInfo;

    void Awake()
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
    void Start()
    {

        GridMatrixOfTotalDisplayCards = new List<MatrixOfCards>(CardsManagerScript.instance.GridMatrixOfTotalDisplayCards);

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
                positionsForLine.Add((GridMatrixOfTotalDisplayCards.Find(o => o.row == solutionInfo.data[j].row && o.column == solutionInfo.data[j].column).Card.transform.position));
            }
            if (solutionInfo.length == 5)
            {
                LineRenderer lineRenderer = Instantiate(LinePrefab, transform).GetComponent<LineRenderer>();
                lineRenderer.SetPositions(positionsForLine.ToArray());
            }
            else if (solutionInfo.length >= 5)
            {
                LineRenderer lineRenderer = Instantiate(LinePrefab, transform).GetComponent<LineRenderer>();
                lineRenderer.SetPositions(positionsForLine.GetRange(0, 5).ToArray());
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
        }

        MatrixOfCards grid = GridMatrixOfTotalDisplayCards.Find(x => x.Card.Equals(Card));
        Debug.Log("Score Update Start");
        Debug.Log("row :" + grid.row + " column: " + grid.column + " card: " + grid.Card.name + " value " + value + " Score " + ScoreOfCards[grid.row, grid.column]);
        ScoreOfCards[grid.row, grid.column] = value;
        Debug.Log("row :" + grid.row + " column: " + grid.column + " card: " + grid.Card.name + " value " + value + " Score " + ScoreOfCards[grid.row, grid.column]);
        Debug.Log("Score Update end");
    }



}
