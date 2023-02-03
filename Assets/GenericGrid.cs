using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GenericGrid<T>
{
    /// <summary>The secondary dimension list</summary>
    [SerializeField] public List<T> data = new List<T>();
    [SerializeField] public int rows;
    [SerializeField] public int cols;

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

    /// <summary>The number of elements in the list</summary>
    public int Count => data.Count;
}