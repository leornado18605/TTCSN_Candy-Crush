using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GridPosition
{
    public int x;
    public int y;
    public GridPosition(int x, int y) { this.x = x; this.y = y; }

    public static readonly GridPosition Invalid = new GridPosition(-1, -1);
    public bool IsValid => x >= 0 && y >= 0;
}
