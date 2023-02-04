using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int row;
    public int col;
    public float size;

    public int PlantLevel
    {
        set
        {
            if (_plantLevel == value)
            {
                return;
            }

            _plantLevel = value;

            var plantSpritesTable = SOManager.Instance.PlantSprites.Table;

            _plantLevel = Mathf.Min(_plantLevel, plantSpritesTable.Length - 1);
            _plantLevel = Mathf.Max(_plantLevel, 0);
            
            _spriteRenderer.sprite = plantSpritesTable[_plantLevel];
        }
        get => _plantLevel;
    }

    private int _plantLevel = 0;
    
    public Dictionary<Direction, Cell> Neighbours = new Dictionary<Direction, Cell>();

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public enum Direction {
        Up,
        Down,
        Left,
        Right
    }
}
