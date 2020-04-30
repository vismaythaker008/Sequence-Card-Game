using System.Collections.Generic;
using UnityEngine;

static class Solution
{

    public static SolutionInfo longestLine(List<ScoreInfo> M, int value)
    {
        M.Find(o => o.row == 0 && o.column == 0).value = value;
        M.Find(o => o.row == 0 && o.column == 9).value = value;
        M.Find(o => o.row == 9 && o.column == 0).value = value;
        M.Find(o => o.row == 9 && o.column == 9).value = value;

        // M[0, 0] = value;
        // M[9, 0] = value;
        // M[0, 9] = value;
        // M[9, 9] = value;
        SolutionInfo solutionInfo = new SolutionInfo();
        SolutionInfo temp;
        solutionInfo.length = 0;
        if (M.Count == 0)
            return null;
        // int m = M.GetLength(0), n = M.GetLength(1);
        int m = 10, n = 10;
        // int len = 0;
        for (int i = 0; i < m; i++)
        {
            if (solutionInfo.length < (temp = check(M, i, 0, new int[] { 0, 1 }, value)).length)
            {
                solutionInfo = temp;
            }

        }

        for (int i = 0; i < n; i++)
        {
            if (solutionInfo.length < (temp = check(M, 0, i, new int[] { 1, 0 }, value)).length)
            {
                solutionInfo = temp;
            }

        }

        for (int i = 0; i < m; i++)
        {
            if (solutionInfo.length < (temp = check(M, i, 0, new int[] { 1, 1 }, value)).length)
            {
                solutionInfo = temp;
            }

            if (solutionInfo.length < (temp = check(M, i, n - 1, new int[] { 1, -1 }, value)).length)
            {
                solutionInfo = temp;
            }

        }

        for (int i = 1; i < n; i++)
        {
            if (solutionInfo.length < (temp = check(M, 0, i, new int[] { 1, 1 }, value)).length)
            {
                solutionInfo = temp;
            }
            if (solutionInfo.length < (temp = check(M, 0, n - i - 1, new int[] { 1, -1 }, value)).length)
            {
                solutionInfo = temp;
            }
        }
        return solutionInfo;
    }

    private static SolutionInfo check(List<ScoreInfo> matrix, int row, int col, int[] dir, int value)
    {
        int len = 0, count = 0;
        SolutionInfo solutionInfo = new SolutionInfo();

        for (; row >= 0 && col >= 0 && row < 10/*matrix.GetLength(0)*/ && col < 10/*matrix.GetLength(1)*/; row += dir[0], col += dir[1])
        {
            if (matrix.Find(o => o.row == row && o.column == col).value == value)
            {
                count++;
                len = Mathf.Max(len, count);
                RowColumnInfo rowColumnInfo = new RowColumnInfo(row, col);
                solutionInfo.data.Add(rowColumnInfo);
                solutionInfo.length = len;
            }
            else
            {
                count = 0;
            }
        }

        return solutionInfo;
    }
}
[System.Serializable]
public class SolutionInfo
{
    public int length;
    public List<RowColumnInfo> data = new List<RowColumnInfo>();
}

[System.Serializable]
public class RowColumnInfo
{
    public int row;
    public int column;

    public RowColumnInfo(int r, int c)
    {
        row = r;
        column = c;
    }
}
