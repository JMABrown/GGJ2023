using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public Camera MainCamera;
    public LayerMask RaycastableMask;
    public float CameraMoveSpeed;

    [FormerlySerializedAs("GameBoard")] public GameObject GameBoardParent;
    public GameObject CellPrefab;

    public int NumRows;
    public int NumCols;

    private Vector3 _lastRightClickMousePosition;
    private Vector3 _lastRightClickCameraPosition;
    
    public GenericGrid<Cell> GameBoard;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }

            return _instance;
        }
    }
    
    private static GameManager _instance;
    
    void Awake()
    {
        GameBoard = new GenericGrid<Cell>(NumRows, NumCols);
        for (int i = 0; i < NumRows * NumCols; i++)
        {
            GameBoard.data.Add(null);
        }
        
        for (int row = 0; row < NumRows; row++) {
            for (int col = 0; col < NumCols; col++) {
                var newCellObject = Instantiate(CellPrefab, GameBoardParent.transform);
                var newCell = newCellObject.GetComponent<Cell>();
                newCellObject.transform.position = new Vector3(col * newCell.size, row * newCell.size);
                newCell.row = row;
                newCell.col = col;
                GameBoard[row, col] = newCell;
            }
        }
        
        for (int r = 1; r < NumRows-1; r++) {
            for (int c = 1; c < NumCols-1; c++) {
                GameBoard[r, c].Neighbours[Cell.Direction.Up] = GameBoard[r + 1, c];
                GameBoard[r, c].Neighbours[Cell.Direction.Down] = GameBoard[r - 1, c];
                GameBoard[r, c].Neighbours[Cell.Direction.Left] = GameBoard[r, c - 1];
                GameBoard[r, c].Neighbours[Cell.Direction.Right] = GameBoard[r, c + 1];
            }
        }

        GameBoard.GetRandomCell().PlantLevel = 1;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Input.mousePosition
            //mainCamera.ScreenToWorldPoint

            RaycastHit hit;
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
            //if (Physics.Raycast(ray, out hit, 100.0f, RaycastableMask))
            //if (Physics2D.Raycast(MainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero)) ;
            //{
            //    if (hit.transform.TryGetComponent<Cell>(out Cell hitCell))
            //    {
            //        Debug.Log("Row:" + hitCell.row + ", Col:" + hitCell.col);
            //    }
            //}
            RaycastHit2D hit2D = Physics2D.Raycast(MainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit2D.collider != null)
            {
                if (hit2D.transform.TryGetComponent(out Cell hitCell))
                {
                    //Debug.Log("Row:" + hitCell.row + ", Col:" + hitCell.col);
                    if (hitCell.Neighbours[Cell.Direction.Up].PlantLevel > 0
                        || hitCell.Neighbours[Cell.Direction.Down].PlantLevel > 0
                        || hitCell.Neighbours[Cell.Direction.Left].PlantLevel > 0
                        || hitCell.Neighbours[Cell.Direction.Right].PlantLevel > 0
                        || hitCell.PlantLevel > 0)
                    {
                        hitCell.PlantLevel += 1;
                    }
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
