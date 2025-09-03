using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragSphere : MonoBehaviour
{
    private float rotationSpeed = 50f; // tốc độ xoay
    private Vector3 lastMousePosition;
    private bool isDragging = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform) 
                {
                    isDragging = true;
                    lastMousePosition = Input.mousePosition;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;

            float rotationX = -delta.y * rotationSpeed * Time.deltaTime;

            transform.Rotate(rotationX, 0f, 0f, Space.World);

            lastMousePosition = Input.mousePosition;
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.transform == transform)
                    {
                        isDragging = true;
                        lastMousePosition = touch.position;
                    }
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                Vector2 delta = touch.deltaPosition;

                float rotationX = -delta.y * rotationSpeed * Time.deltaTime;

                transform.Rotate(rotationX, 0f, 0f, Space.World);

                lastMousePosition = touch.position;
            }
        }
    }
}
