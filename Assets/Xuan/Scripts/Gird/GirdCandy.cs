using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GirdCandy : Singleton<GirdCandy>
{
    [SerializeField] private Candy candyPrefab;
    [SerializeField] private Transform candyParent;
    [SerializeField] private CandyData candyData;
    private int width = 9;
    private int height = 9;
    private Vector2 callSize;

    private int x0;
    private int y0;
    private float timeDelayMove = 0.3f;

    private Candy[,] gridCandy;

    private void Awake()
    {
        callSize = candyPrefab.GetComponent<SpriteRenderer>().bounds.size;
        callSize.x += 0.03f;
        callSize.y += 0.03f;
        gridCandy = new Candy[width, height];
        SetPos();
        SpawnCandy();
    }

    public void SetGridCandy(Candy candy)
    {
        if (candy == null) return;
        if (gridCandy == null) return;
        if (candy.row < 0 || candy.row >= width || candy.column < 0 || candy.column >= height) return;

        gridCandy[candy.row, candy.column] = candy;
    }
    public void SpawnCandy()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if (gridCandy[i, j] == null)
                {
                    Candy candy = PoolingManager.Spawn(candyPrefab, SetPosSpawnCandy(i,j), Quaternion.identity, candyParent);

                    int index = Random.Range(0, candyData.candies.Count);
                    candy.Init(candyData.candies[index], i, j);
                    SetGridCandy(candy);
                }
            }
        }
    }
    public void SetPos()
    {
        x0 = width / 2;
        y0 = height / 2;
    }
    public Vector3 SetPosSpawnCandy(int column, int row)
    {
        int x = row - x0;
        int y = column - y0;

        
        Vector3 pos = new Vector3(x * callSize.x, y * callSize.y, 0f);

        return pos;
    }

    public bool CheckCandy(Candy candy)
    {
        if (gridCandy[candy.row, candy.column] == null) return false;

        var horizontal = FindMatches(candy, new Vector2Int(1, 0));
        if (horizontal.Count >= 3)
        {
            DeleteCandy(horizontal, false);
            return true;
        }

        var vertical = FindMatches(candy, new Vector2Int(0, 1));
        if (vertical.Count >= 3)
        {
            DeleteCandy(vertical, true);
            return true;
        }
        return false;
    }

    public void DeleteCandy(List<Candy> candyCheck, bool isHorizontal)
    {
        List<Vector2Int> listCheck = new List<Vector2Int>();
        int indexY = 9;
        foreach(Candy candy in candyCheck)
        {
            if (indexY > candy.row) indexY = candy.row;
            listCheck.Add(new Vector2Int(candy.row, candy.column));
            DeleteCandy(candy);
        }

        if(!isHorizontal)
        {
            CheckGirdCandy(indexY, candyCheck[0].column);
        }
        else
        {
            foreach (Vector2Int candy in listCheck)
            {
                CheckGirdCandy(candy.x, candy.y);
            }
        }
    }
    public void DeleteCandy(Candy candy)
    {
        gridCandy[candy.row, candy.column] = null;
        int r = candy.row;
        int c = candy.column;
        PoolingManager.Despawn(candy.gameObject);

    }
    private List<Candy> FindMatches(Candy candy, Vector2Int dir)
    {
        List<Candy> result = new List<Candy>();
        result.Add(candy);

        // Quét xuôi
        int x = candy.row + dir.x;
        int y = candy.column + dir.y;

        while (x >= 0 && x < width && y >= 0 && y < height && gridCandy[x, y] != null)
        {
            if (gridCandy[x, y].Type != candy.Type) break;

            result.Add(gridCandy[x, y]);
            x += dir.x;
            y += dir.y;
        }

        // Quét ngược
        x = candy.row - dir.x;
        y = candy.column - dir.y;
        while (x >= 0 && x < width && y >= 0 && y < height && gridCandy[x, y] != null)
        {
            if (gridCandy[x, y].Type != candy.Type) break;

            result.Add(gridCandy[x, y]);
            x -= dir.x;
            y -= dir.y;
        }

        return result;
    }

    public void CheckGirdCandy(int r, int c)
    {

        for (int row = r; row < height; row++)
        {
            if (gridCandy[row, c] == null)
            {
                int aboveRow = row + 1;
                while (aboveRow < height && gridCandy[aboveRow, c] == null)
                {
                    aboveRow++;
                }

                int targetRow = row;
                int targetCol = c;

                if (aboveRow < height)
                {
                    // Có kẹo ở trên
                    Candy candy = gridCandy[aboveRow, targetCol];
                    gridCandy[aboveRow, targetCol] = null;

                    Sequence seq = DOTween.Sequence();
                    seq.Append(candy.transform.DOMove(SetPosSpawnCandy(targetRow, targetCol), timeDelayMove));
                    seq.OnComplete(() =>
                    {
                        // Check index an toàn
                        if (targetRow >= 0 && targetRow < height &&
                            targetCol >= 0 && targetCol < width)
                        {
                            candy.SetPosition(targetRow, targetCol);
                            gridCandy[targetRow, targetCol] = candy;
                        }

                        seq.Kill();
                    });
                }
                else
                {
                    // Spawn kẹo mới
                    Candy newCandy = PoolingManager.Spawn(
                        candyPrefab,
                        SetPosSpawnCandy(height, targetCol),
                        Quaternion.identity,
                        candyParent
                    );

                    int index = Random.Range(0, candyData.candies.Count);
                    newCandy.Init(candyData.candies[index], targetRow, targetCol);

                    gridCandy[targetRow, targetCol] = newCandy;

                    Sequence seq = DOTween.Sequence();
                    seq.Append(newCandy.transform.DOMove(SetPosSpawnCandy(targetRow, targetCol), timeDelayMove));
                    seq.OnComplete(() => seq.Kill());
                }
            }
        }
/*        Debug.Log("--------Check--------");
        for (int row = r; row < height; row++)
        {
            CheckCandy(gridCandy[row, c]);
        }
*/
    }

}
