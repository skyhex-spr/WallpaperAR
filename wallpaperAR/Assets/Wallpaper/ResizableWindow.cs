using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizableWindow : MonoBehaviour
{
    public GameObject windowPrefab;
    private Vector3 startDragPosition;
    private bool isDragging = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            isDragging = true;
            startDragPosition = GetCursorPosition();
           
        }

        if (isDragging)
        {
            if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
            {
                Vector3 endDragPosition = GetCursorPosition();
                InstantiateWindow(startDragPosition, endDragPosition);
                isDragging = false;
            }
        }
    }

    private Vector3 GetCursorPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }

        return Vector3.zero;
    }

    private void InstantiateWindow(Vector3 startPosition, Vector3 endPosition)
    {
        Vector3 windowPosition = (startPosition + endPosition) / 2.0f;
        Vector3 windowScale = new Vector3(
            endPosition.x - startPosition.x,
            endPosition.y - startPosition.y,
            windowPrefab.transform.localScale.z
        );

        GameObject newWindow = Instantiate(windowPrefab, windowPosition, Quaternion.identity);
        newWindow.transform.localScale = windowScale;
        newWindow.transform.position = new Vector3(newWindow.transform.position.x, newWindow.transform.position.y, startPosition.z - 0.01f);
    }
}
