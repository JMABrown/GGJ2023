using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class TroopBehaviour : MonoBehaviour
{
    private GenericGrid<Cell> _board;

    private bool _moving = false;
    
    private bool _dead = false;

    private Coroutine _troopMovementRoutine;
    private Coroutine _troopShootRoutine;

    private Vector3 _wanderPosition;

    public LineRenderer _bulletLine;
    
    void Start()
    {
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
            Debug.Log($"Position: {position}");
            var simplePosition = new Vector2(Mathf.Floor(position.x), Mathf.Floor(position.y));
            Debug.Log($"SimplePosition: {simplePosition}");
            
            Cell validWanderCell = null;
            var cellTrooperIsIn = _board[(int)simplePosition.y, (int)simplePosition.x];
            Debug.Log($"cellTrooperIsIn: {cellTrooperIsIn}");
            List<Cell> possibleWanderCells = new List<Cell>();

            var neighbouringCell = cellTrooperIsIn.Neighbours[Cell.Direction.Up];
            if (neighbouringCell != null)
            {
                Debug.Log("Adding wander cell: UP");
                possibleWanderCells.Add(neighbouringCell);
            }
            neighbouringCell = cellTrooperIsIn.Neighbours[Cell.Direction.Down];
            if (neighbouringCell != null)
            {
                Debug.Log("Adding wander cell: DOWN");
                possibleWanderCells.Add(neighbouringCell);
            }
            neighbouringCell = cellTrooperIsIn.Neighbours[Cell.Direction.Left];
            if (neighbouringCell != null)
            {
                Debug.Log("Adding wander cell: LEFT");
                possibleWanderCells.Add(neighbouringCell);
            }
            neighbouringCell = cellTrooperIsIn.Neighbours[Cell.Direction.Right];
            if (neighbouringCell != null)
            {
                Debug.Log("Adding wander cell: RIGHT");
                possibleWanderCells.Add(neighbouringCell);
            }

            var randomChoice = Random.Range(0, possibleWanderCells.Count);
            var randomChoiceCell = possibleWanderCells[randomChoice];
            Debug.Log($"Possible wander cells: {possibleWanderCells}");
            Debug.Log($"Random choice: {randomChoice}");

            if (randomChoiceCell.PlantLevel != 0)
            {
                Debug.Log("Random choice has plant level");
                continue;
            }
            
            var wanderCellPosition = randomChoiceCell.transform.position;
            //var randomNoise = Random.insideUnitCircle;
            var randomNoise = Vector2.zero;
            _wanderPosition = new Vector3(wanderCellPosition.x + randomNoise.x*0.2f,
                wanderCellPosition.y + randomNoise.y*0.2f,
                0f);
            Debug.Log($"New _wanderPosition: {wanderCellPosition}");
            
            _moving = true;
            Debug.Log($"Starting move...");
            DOTween.To(() => transform.position, x => transform.position = x, _wanderPosition, Globals.WanderTime)
                .SetEase(Ease.Linear).onComplete += OnMoveComplete;

            yield return new WaitUntil(() => !_moving);
            
        }
    }

    private IEnumerator ShootingBrain()
    {
        if (Random.Range(0, 10) < 2)
        {
            _board = GameManager.Instance.GameBoard;
            var position = transform.position;
            var simplePosition = new Vector2(Mathf.Floor(position.x), Mathf.Floor(position.y));
            var cellTrooperIsIn = _board[(int)simplePosition.y, (int)simplePosition.x];

            foreach (var cell in cellTrooperIsIn.Neighbours)
            {
                foreach (var cellOfCell in cell.Value.Neighbours)
                {
                    if (cellOfCell.Value.PlantLevel > 0)
                    {
                        //_bulletLine.
                    }
                }
            }
        }
        yield return null;
    }

    private void OnMoveComplete()
    {
        _moving = false;
        Debug.Log($"Move complete");
    }
}
