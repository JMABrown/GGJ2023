using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class GenericGrid<T>
{
    /// <summary>The secondary dimension list</summary>
    [SerializeField] public List<T> data;
    [SerializeField] public int rows;
    [SerializeField] public int cols;
    
    public GenericGrid(int rowsIn, int colsIn)
    {
        data = new List<T>(rowsIn * colsIn);
        rows = rowsIn;
        cols = colsIn;
    }

    /// <summary>List indexer</summary>
    public T this[int i]
    {
        get => data[i];
        set => data[i] = value;
    }

    public T this[int r, int c]
    {
        get => data[r * cols + c];
        set => data[r * cols + c] = value;
    }

    public List<T> GetRow(int r)
    {
        List<T> list = new List<T>(cols);

        for (int c = 0; c < cols; c++)
        {
            list.Add(data[r * cols + c]);
        }
        return list;
    }

    public List<T> GetCol(int c)
    {
        List<T> list = new List<T>(rows);

        for (int r = 0; r < rows; r++)
        {
            list.Add(data[r * cols + c]);
        }
        return list;
    }

    public T GetRandomCell()
    {
        int randCol = Random.Range(0, rows-1);
        int randRow = Random.Range(0, cols-1);
        return this[randRow, randCol];
    }

    /// <summary>The number of elements in the list</summary>
    public int Count => data.Count;
}