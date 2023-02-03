using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera MainCamera;
    public LayerMask RaycastableMask;
    public float CameraMoveSpeed;

    private Vector3 _lastRightClickMousePosition;
    private Vector3 _lastRightClickCameraPosition;

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
