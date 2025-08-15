using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public GridPosition pos;
    public bool isBlocked;
    public Candy candy; // can be null

    public void SetCandy(Candy c)
    {
        candy = c;

        if (c != null)
        {
            c.CurrentCell = this;
            c.transform.SetParent(transform, worldPositionStays: true);
            c.transform.localPosition = Vector3.zero;
        }
    }
}
