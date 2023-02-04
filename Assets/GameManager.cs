using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera MainCamera;
    public LayerMask RaycastableMask;
    public float CameraMoveSpeed;

    public GameObject GameBoard;
    public GameObject CellPrefab;

    public int NumRows;
    public int NumCols;

    private Vector3 _lastRightClickMousePosition;
    private Vector3 _lastRightClickCameraPosition;
    
    private GenericGrid<Cell> _board;
    
    void Awake()
    {
        _board = new GenericGrid<Cell>(NumRows, NumCols);
        for (int i = 0; i < NumRows * NumCols; i++)
        {
            _board.data.Add(null);
        }
        
        for (int row = 0; row < NumRows; row++) {
            for (int col = 0; col < NumCols; col++) {
                Debug.Log("Row: " + row + "Col: " + col);
                var newCellObject = Instantiate(CellPrefab, GameBoard.transform);
                var newCell = newCellObject.GetComponent<Cell>();
                newCellObject.transform.position = new Vector3(col * newCell.size, row * newCell.size);
                newCell.row = row;
                newCell.col = col;
                _board[row, col] = newCell;
            }
        }
        
        for (int r = 1; r < NumRows-1; r++) {
            for (int c = 1; c < NumCols-1; c++) {
                _board[r, c].Neighbours[Cell.Direction.Up] = _board[r + 1, c];
                _board[r, c].Neighbours[Cell.Direction.Down] = _board[r - 1, c];
                _board[r, c].Neighbours[Cell.Direction.Left] = _board[r, c - 1];
                _board[r, c].Neighbours[Cell.Direction.Right] = _board[r, c + 1];
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Input.mousePosition
            //mainCamera.ScreenToWorldPoint

            RaycastHit hit;
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f, RaycastableMask))
            {
                if (hit.transform.TryGetComponent<Cell>(out Cell hitCell))
                {
                    //hitCell
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            _lastRightClickMousePosition = Input.mousePosition;
            _lastRightClickCameraPosition = MainCamera.transform.position;
        }

        if (Input.GetMouseButton(1))
        {
            var rightClickPosition = Input.mousePosition;
            var mouseDelta = rightClickPosition - _lastRightClickMousePosition;
            MainCamera.transform.position = new Vector3(
                _lastRightClickCameraPosition.x + mouseDelta.x * CameraMoveSpeed,
                _lastRightClickCameraPosition.y + mouseDelta.y * CameraMoveSpeed,
                _lastRightClickCameraPosition.z + mouseDelta.z * CameraMoveSpeed);
        }
    }
}
