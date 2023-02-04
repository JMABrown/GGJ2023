using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int row;
    public int col;
    public float size;
    
    public Dictionary<Direction, Cell> Neighbours = new Dictionary<Direction, Cell>();
    
    public enum Direction {
        Up,
        Down,
        Left,
        Right
    }
}
