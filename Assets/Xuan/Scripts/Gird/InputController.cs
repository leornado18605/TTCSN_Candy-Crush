using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public Camera cam;

    private Cell selected;
    private Vector3 pressWorld;

    void Update()
    {
        if (GirdCandy.Instance.IsBusy) return;

        if (Input.GetMouseButtonDown(0))
        {
            pressWorld = cam.ScreenToWorldPoint(Input.mousePosition);
            pressWorld.z = 0f;
            selected = RaycastCell(pressWorld);
        }
        else if (Input.GetMouseButtonUp(0) && selected != null)
        {
            Vector3 releaseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
            releaseWorld.z = 0f;
            Vector2 delta = releaseWorld - pressWorld;
            if (delta.magnitude > 0.2f)
            {
                GridPosition dir = AbsMaxDirection(delta);
                var neighbor = GirdCandy.Instance.GetType().GetMethod("GetCell", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(GirdCandy.Instance, new object[] { selected.pos.x + dir.x, selected.pos.y + dir.y }) as Cell;
                if (neighbor != null)
                {
                    StartCoroutine(GirdCandy.Instance.TrySwap(selected, neighbor));
                }
            }
            selected = null;
        }
    }

    Cell RaycastCell(Vector3 worldPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.down);
        if (hit.collider != null)
        {
            return hit.collider.GetComponent<Cell>();
        }
        return null;
    }

    GridPosition AbsMaxDirection(Vector2 v)
    {
        if (Mathf.Abs(v.x) > Mathf.Abs(v.y))
            return new GridPosition(v.x > 0 ? 1 : -1, 0);
        else
            return new GridPosition(0, v.y > 0 ? 1 : -1);
    }
}
