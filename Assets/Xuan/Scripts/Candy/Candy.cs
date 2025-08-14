using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CandyType
{
    Red,
    Blue,
    Green,
    Yellow,
    Purple,
    Orange,
    //Update
}
public class Candy : MonoBehaviour
{
    [Header("Candy")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    private CandyType candyType;
    public CandyType Type => candyType;


    public int row;
    public int column;

    public void Init(CandyInfo candy, int r , int c)
    {
        candyType = candy.candyType;
        spriteRenderer.sprite = candy.candyIcon;

        SetPosition(r, c);
    }
    public void SetPosition(int r, int c)
    {
        row = r;
        column = c;
    }
}
