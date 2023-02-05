using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class TroopBehaviour : MonoBehaviour
{
    private GenericGrid<Cell> _board;

    private List<Cell.Direction> _directions;

    private bool _moving = false;
    
    private bool _dead = false;

    private Coroutine _troopMovementRoutine;
    private Coroutine _troopShootRoutine;

    private Vector3 _wanderPosition;
    
    void Start()
    {
        _directions = new List<Cell.Direction>(4);
        _directions.Add(Cell.Direction.Up);
        _directions.Add(Cell.Direction.Down);
        _directions.Add(Cell.Direction.Left);
        _directions.Add(Cell.Direction.Right);

        _board = GameManager.Instance.GameBoard;
        _troopMovementRoutine = StartCoroutine(TroopMovementBrain());
    }

    private IEnumerator TroopMovementBrain()
    {
        while (!_dead)
        {
            yield return null;

            _board = GameManager.Instance.GameBoard;
            
            var position = transform.position;
            var simplePosition = new Vector2(Mathf.Floor(position.x), Mathf.Floor(position.y));
            var randomDirections = new List<Cell.Direction>(_directions.Count);
            int originalDirectionsCount = Int32.Parse(_directions.Count.ToString()); // Some hack to break a strange reevaluation of this
            for (int i = 0; i < originalDirectionsCount; i++)
            {
                var randIndex = Random.Range(0, _directions.Count);
                randomDirections.Add(_directions[randIndex]);
                _directions.RemoveAt(randIndex);
            }
            _directions = randomDirections;

            Cell validWanderCell = null;
            for (int i = 0; i < randomDirections.Count; i++)
            {
                var selectedRandomDirection = randomDirections[i];
                var cellTrooperIsIn = _board[(int)simplePosition.y, (int)simplePosition.x];
                var wanderCell = cellTrooperIsIn.Neighbours[selectedRandomDirection];
                if (wanderCell == null)
                {
                    continue;
                }
                if (wanderCell.PlantLevel == 0)
                {
                    validWanderCell = wanderCell;
                    break;
                }
            }

            if (validWanderCell == null)
            {
                continue;
            }
            
            var wanderCellPosition = validWanderCell.transform.position;
            _wanderPosition = new Vector3(wanderCellPosition.x + Random.insideUnitCircle.x*0.2f,
                wanderCellPosition.y + Random.insideUnitCircle.y*0.2f,
                0f);
            
            _moving = true;
            DOTween.To(() => transform.position, x => transform.position = x, _wanderPosition, Globals.WanderTime)
                .SetEase(Ease.Linear).onComplete += OnMoveComplete;

            yield return new WaitUntil(() => !_moving);
        }
    }

    private void OnMoveComplete()
    {
        _moving = false;
    }
}
