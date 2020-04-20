using UnityEngine;

static class Solution
{

    public static int longestLine(int[,] M, int value)
    {
        if (M.Length == 0) return 0;
        int m = M.GetLength(0), n = M.GetLength(1);
        int len = 0;
        for (int i = 0; i < m; i++)
        {
            len = Mathf.Max(len, check(M, i, 0, new int[] { 0, 1 }, value));
        }

        for (int i = 0; i < n; i++)
        {
            len = Mathf.Max(len, check(M, 0, i, new int[] { 1, 0 }, value));
        }

        for (int i = 0; i < m; i++)
        {
            len = Mathf.Max(len, check(M, i, 0, new int[] { 1, 1 }, value));
            len = Mathf.Max(len, check(M, i, n - 1, new int[] { 1, -1 }, value));
        }

        for (int i = 1; i < n; i++)
        {
            len = Mathf.Max(len, check(M, 0, i, new int[] { 1, 1 }, value));
            len = Mathf.Max(len, check(M, 0, n - i - 1, new int[] { 1, -1 }, value));
        }

        return len;
    }

    private static int check(int[,] matrix, int row, int col, int[] dir, int value)
    {
        int len = 0, count = 0;

        for (; row >= 0 && col >= 0 && row < matrix.GetLength(0) && col < matrix.GetLength(1); row += dir[0], col += dir[1])
        {
            if (matrix[row, col] == value)
            {
                count++;
                len = Mathf.Max(len, count);
            }
            else
            {
                count = 0;
            }
        }

        return len;
    }
}