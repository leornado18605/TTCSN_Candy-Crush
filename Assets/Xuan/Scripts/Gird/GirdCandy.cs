using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class GirdCandy : Singleton<GirdCandy>
{
    [Header("Candy")]
    [SerializeField] private Candy candyPrefab;
    [SerializeField] private CandyDataController dataController;

    [Header("Cell")]
    [SerializeField] private Cell cellPrefabs;
    [SerializeField] private Transform cellParent;
    public Vector2 cellSize;

    [Header("Level Default")]
    [SerializeField] private LevelData level;

    [Header("UI")]
    [SerializeField] private LoseUI loseUI;
    [SerializeField] private WinUI winUI;

    [Header("Timing")]
    public float swapDuration = 0.15f;
    public float dropDurationPerCell = 0.06f;
    public float refillDelay = 0.05f;

    [Header("Sorce")]
    private Cell[,] cells;
    private int score;
    private int movesLeft;

    private int width;
    private int height;
    private System.Random rng = new System.Random();

    public bool IsBusy { get; set; }
    public int Score => score;

    private void OnDisable()
    {
        OutGame();
        AudioController.Instance.StopMusic();
    }

    public void InitStartGame(LevelData lel = null)
    {
        AudioController.Instance.PlayAudioGamePlay();
        //ClearGrid();
        if(lel == null && level == null)
        {
            UnityEngine.Debug.LogError("No Level Data Found");
            return;
        }

        if (lel != null) this.level = lel;

        score = 0;
        movesLeft = level.moveLimit;

        ObserverManager<TextID>.PostEven(TextID.SetupScore, level.targetScore);
        ObserverManager<TextID>.PostEven(TextID.ChangeMoveLimit, movesLeft);

        width = level.width;
        height = level.height;

        cells = new Cell[width, height];

        cellSize = cellPrefabs.GetComponent<SpriteRenderer>().bounds.size;
        //cellSize.x += 0.03f;
        //cellSize.y += 0.03f;


        Debug.Log("Spawn Cell Grid");
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Cell newCell = PoolingManager.Spawn(cellPrefabs, transform.position, Quaternion.identity, cellParent);
                newCell.name = $"Cell_{x}_{y}";
                newCell.transform.position = CellToWorld(new GridPosition(x, y));
                
                newCell.pos = new GridPosition(x, y);
                newCell.isBlocked = false;
                newCell.candy = null;
                cells[x, y] = newCell;
            }
        }
        StartCoroutine(FillBoardNoMatches());
    }

    public Vector3 CellToWorld(GridPosition pos)
    {
        int x = width / 2;
        int y = height / 2;

        return new Vector3((pos.x - x) * cellSize.x,(pos.y - y)*cellSize.y , 0f);
    }

    public bool InBounds(int x, int y) => x >= 0 && x < width && y >= 0 && y < height;

    public Candy SpawnRandomCandyAt(int x, int y)
    {
        CandyData def = level.candies[rng.Next(level.candies.Length)];  //lay list candy
        Candy candy = PoolingManager.Spawn(candyPrefab, cells[x, y].transform.position, Quaternion.identity, cells[x, y].transform);

        candy.Init(def.candies.candyType,def.candies.specialType,def.candies.icon);
        cells[x, y].SetCandy(candy);

        return candy;
    }
    IEnumerator FillBoardNoMatches()
    {

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (cells[x, y].isBlocked) continue;

                Candy newCandy;
                int safety = 0;
                do
                {
                    if (cells[x, y].candy != null) PoolingManager.Despawn(cells[x, y].candy.gameObject);
                    newCandy = SpawnRandomCandyAt(x, y);
                    safety++;
                    if (safety > 50) break; 
                }
                while (CreatesImmediateMatch(x, y));
                yield return null;
            }
        }
    }
    bool CreatesImmediateMatch(int x, int y)
    {
        // Check 2 left + this, 2 down + this
        Candy target = cells[x, y].candy;
        if (target == null) return false;
        CandyType color = target.Type;

        // Horizontal
        if (x >= 2)
        {
            Candy c1 = cells[x - 1, y].candy; 
            Candy c2 = cells[x - 2, y].candy;
            if (c1 && c2 && c1.Type == color && c2.Type == color) return true;
        }
        // Vertical
        if (y >= 2)
        {
            Candy c1 = cells[x, y - 1].candy;
            Candy c2 = cells[x, y - 2].candy;
            if (c1 && c2 && c1.Type == color && c2.Type == color) return true;
        }
        return false;
    }

    public IEnumerator TrySwap(Cell a, Cell b)
    {
        if (IsBusy) yield break;
        bool isCheck = false;

        if (a == null || b == null || a.candy == null || b.candy == null) yield break;
        if (!AreAdjacent(a.pos, b.pos)) yield break;

        IsBusy = true;
        yield return SwapCandies(a, b);

        if(a.candy.SpecialType == CandySpecialType.ColorBomb || b.candy.SpecialType == CandySpecialType.ColorBomb)
        {
            Candy targetCandy = a.candy.SpecialType == CandySpecialType.ColorBomb ? b.candy : a.candy;
            Cell c = a.candy.SpecialType != CandySpecialType.ColorBomb ? b : a;
            isCheck = true;

            StartCoroutine(c.candy.AnimatePop(0.15f));
            c.candy = null;

            yield return ClearGridByColor(targetCandy.Type);
        }

        var matches = FindAllMatches();
        if (matches.Count == 0 && !isCheck)
        {
            // swap back
            yield return SwapCandies(a, b);
            IsBusy = false;
            CheckMoveLimit();
            yield break;
        }

        ChangeMoveLeft(-1);

        // Resolve cascades
        yield return ResolveMatchesAndCascades(matches);
        IsBusy = false;
    }

    bool AreAdjacent(GridPosition p1, GridPosition p2)
    {
        return (p1.x == p2.x && Mathf.Abs(p1.y - p2.y) == 1) || (p1.y == p2.y && Mathf.Abs(p1.x - p2.x) == 1);
    }

    IEnumerator SwapCandies(Cell a, Cell b)
    {
        Candy ca = a.candy; Candy cb = b.candy;
        if (ca == null || cb == null) yield break;

        Vector3 wa = ca.transform.position;
        Vector3 wb = cb.transform.position;

        // animate
        var co1 = StartCoroutine(ca.AnimateMoveTo(wb, swapDuration));
        var co2 = StartCoroutine(cb.AnimateMoveTo(wa, swapDuration));
        yield return co1; yield return co2;

        // reparent
        a.SetCandy(cb);
        b.SetCandy(ca);
    }

    List<HashSet<Cell>> FindAllMatches()
    {
        bool[,] visited = new bool[width,height];
        var clusters = new List<HashSet<Cell>>();

        for (int y = 0; y < height; y++)
        {
            int runStart = 0;
            for (int x = 0; x <= width; x++)
            {
                bool same = false;
                if (x < width && cells[x, y].candy != null)
                {
                    if (x == runStart) { same = true; }
                    else
                    {
                        var color = cells[runStart, y].candy?.Type;
                        same = cells[x, y].candy?.Type == color;
                    }
                }
                if (!same)
                {
                    int len = x - runStart;
                    if (len >= 3)
                    {
                        var set = new HashSet<Cell>();
                        for (int i = runStart; i < x; i++) set.Add(cells[i, y]);
                        clusters.Add(set);
                    }
                    runStart = x;
                }
            }
        }

        // Vertical runs
        for (int x = 0; x < width; x++)
        {
            int runStart = 0;
            for (int y = 0; y <= height; y++)
            {
                bool same = false;
                if (y < height && cells[x, y].candy != null)
                {
                    if (y == runStart) { same = true; }
                    else
                    {
                        var color = cells[x, runStart].candy?.Type;
                        same = cells[x, y].candy?.Type == color;
                    }
                }
                if (!same)
                {
                    int len = y - runStart;
                    if (len >= 3)
                    {
                        var set = new HashSet<Cell>();
                        for (int i = runStart; i < y; i++) set.Add(cells[x, i]);
                        clusters.Add(set);
                    }
                    runStart = y;
                }
            }
        }

        for (int i = 0; i < clusters.Count; i++)
            for (int j = i + 1; j < clusters.Count; j++)
            {
                foreach (var c in clusters[i])
                {
                    if (clusters[j].Contains(c))
                    {
                        clusters[i].UnionWith(clusters[j]);
                        clusters.RemoveAt(j);
                        j--;
                        break;
                    }
                }
            }

        // Filter by >=3
        clusters.RemoveAll(s => s.Count < 3);
        return clusters;
    }

    IEnumerator ResolveMatchesAndCascades(List<HashSet<Cell>> matches)
    {
        do
        {
            yield return HandleMatches(matches);
            yield return CollapseColumns();
            yield return RefillBoard();
            matches = FindAllMatches();
        } while (matches.Count > 0);
        CheckMoveLimit();
    }

    IEnumerator HandleMatches(List<HashSet<Cell>> matches)
    {

        foreach (var cluster in matches)
        {
            Cell specialCell = DetermineSpecialCreationCell(cluster, out CandySpecialType specialType);

            int baseScore = 60; // per candy
            int gained = baseScore * cluster.Count;
            ChangeScore(gained);

            // Pop
            foreach (Cell cell in cluster)
            {
                if (cell == specialCell && specialType != CandySpecialType.None)
                    continue; 
                if (cell.candy)
                {
                    if(cell.candy.SpecialType == CandySpecialType.StripedHorizontal || cell.candy.SpecialType == CandySpecialType.StripedVertical)
                    {
                        StartCoroutine(ActivateSpecialAt(cell));
                    }
                    else if( cell.candy.SpecialType == CandySpecialType.None)
                    {
                        StartCoroutine(cell.candy.AnimatePop(0.15f));
                        cell.candy = null;
                    }
                }
            }

            if (specialCell != null && specialType != CandySpecialType.None)
            {
                Debug.Log("SpecialCell is not null");
                if (specialCell.candy == null)
                {
                    var spawned = SpawnRandomCandyAt(specialCell.pos.x, specialCell.pos.y);
                    specialCell.candy = spawned;
                    SetCandySpecial(specialCell, specialType);
                }
                else
                {
                    SetCandySpecial(specialCell, specialType);
                }
            }

            yield return new WaitForSeconds(0.12f);
        }
    }
    public void SetCandySpecial(Cell cell, CandySpecialType specialType)
    {
        if (cell == null || cell.candy == null) return;

        cell.candy.SpecialType = specialType;

        dataController.SetCandySpecial(cell.candy, specialType, cell.candy.Type);
    }

    Cell DetermineSpecialCreationCell(HashSet<Cell> cluster, out CandySpecialType special)
    {
        special = CandySpecialType.None;

        // Count by row/col
        var byRow = new Dictionary<int, int>();
        var byCol = new Dictionary<int, int>();
        foreach (var c in cluster)
        {
            if (!byRow.ContainsKey(c.pos.y)) byRow[c.pos.y] = 0; byRow[c.pos.y]++;
            if (!byCol.ContainsKey(c.pos.x)) byCol[c.pos.x] = 0; byCol[c.pos.x]++;
        }

        bool hasRow5 = false, hasCol5 = false, hasRow4 = false, hasCol4 = false;
        foreach (var kv in byRow) { if (kv.Value >= 5) hasRow5 = true; if (kv.Value == 4) hasRow4 = true; }
        foreach (var kv in byCol) { if (kv.Value >= 5) hasCol5 = true; if (kv.Value == 4) hasCol4 = true; }

        bool isTL = (byRow.Count >= 2 && byCol.Count >= 2 && cluster.Count >= 5); 

        if (hasRow5 || hasCol5)
            special = CandySpecialType.ColorBomb;
        else if (isTL)
            special = CandySpecialType.Wrapped;
        else if (hasRow4)
            special = CandySpecialType.StripedHorizontal;
        else if (hasCol4)
            special = CandySpecialType.StripedVertical;
        else
            special = CandySpecialType.None;

        Cell candidate = null;
        int maxNeighbors = -1;
        foreach (Cell c in cluster)
        {
            int n = 0;
            if (cluster.Contains(GetCell(c.pos.x + 1, c.pos.y))) n++;
            if (cluster.Contains(GetCell(c.pos.x - 1, c.pos.y))) n++;
            if (cluster.Contains(GetCell(c.pos.x, c.pos.y + 1))) n++;
            if (cluster.Contains(GetCell(c.pos.x, c.pos.y - 1))) n++;
            if (n > maxNeighbors) { maxNeighbors = n; candidate = c; }
        }

        return candidate;
    }

    Cell GetCell(int x, int y)
    {
        if (!InBounds(x, y)) return null;
        return cells[x, y];
    }

    IEnumerator CollapseColumns()
    {
        for (int x = 0; x < width; x++)
        {
            int writeY = 0;
            for (int y = 0; y < height; y++)
            {
                if (cells[x, y].isBlocked) { writeY = y + 1; continue; }
                if (cells[x, y].candy != null)
                {
                    if (y != writeY)
                    {
                        // move down
                        var c = cells[x, y].candy;
                        cells[x, y].candy = null;
                        cells[x, writeY].SetCandy(c);
                        StartCoroutine(c.AnimateMoveTo(CellToWorld(new GridPosition(x, writeY)), dropDurationPerCell * (y - writeY)));
                    }
                    writeY++;
                }
            }
        }
        yield return new WaitForSeconds(dropDurationPerCell * height);
    }

    IEnumerator RefillBoard()
    {
        // spawn from top for empty cells
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (cells[x, y].isBlocked) continue;

                if (cells[x, y].candy == null)
                {
                    CandyData def = level.candies[rng.Next(level.candies.Length)];
                    Candy candy = PoolingManager.Spawn(candyPrefab, transform.position, Quaternion.identity, cells[x,y].transform);

                    candy.Init(def.candies.candyType,def.candies.specialType, def.candies.icon);

                    // spawn above board and drop
                    int above = height;
                    Vector3 spawnPos = CellToWorld(new GridPosition(x, above));
                    candy.transform.position = spawnPos;

                    cells[x, y].SetCandy(candy);
                    StartCoroutine(candy.AnimateMoveTo(CellToWorld(new GridPosition(x, y)), dropDurationPerCell * (above - y)));

                    yield return new WaitForSeconds(refillDelay);
                }
            }
        }
        yield return new WaitForSeconds(dropDurationPerCell * height);
    }

    public IEnumerator ActivateSpecialAt(Cell cell)
    {
        if (cell?.candy == null) yield break;

        Candy c = cell.candy;
        switch (c.SpecialType)
        {
            case CandySpecialType.StripedHorizontal:
                yield return ClearGridByRow(cell.pos.y);
                break;
            case CandySpecialType.StripedVertical:
                yield return ClearGridByColumn(cell.pos.x);
                break;
            case CandySpecialType.Wrapped:
                yield return ClearGridByArea(cell.pos, 1);
                break;
            case CandySpecialType.ColorBomb:
                yield return ClearGridByColor(c.Type);
                break;
        }
    }

    //Grid Candy Funcition

    IEnumerator ClearGridByRow(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (cells[x, y].candy != null)
            {
                if( cells[x, y].candy.SpecialType == CandySpecialType.StripedVertical)
                {
                    StartCoroutine(ActivateSpecialAt(cells[x, y]));
                    continue;
                }
                PoolingManager.Despawn(cells[x, y].candy.gameObject);
                cells[x, y].candy = null;

                ChangeScore(60);
            }
        }
        yield return new WaitForSeconds(0.1f);
    }
    IEnumerator ClearGridByColumn(int x)
    {
        for (int y = 0; y < height; y++)
        {
            if (cells[x, y].candy != null)
            {
                if (cells[x, y].candy.SpecialType == CandySpecialType.StripedHorizontal)
                {
                    StartCoroutine(ActivateSpecialAt(cells[x, y]));
                    continue;
                }
                PoolingManager.Despawn(cells[x, y].candy.gameObject);
                cells[x, y].candy = null;

                ChangeScore(60);
            }
        }
        yield return new WaitForSeconds(0.1f);
    }
    IEnumerator ClearGridByArea(GridPosition center, int radius)
    {
        for (int dx = -radius; dx <= radius; dx++)
            for (int dy = -radius; dy <= radius; dy++)
            {
                int x = center.x + dx; int y = center.y + dy;

                if (!InBounds(x, y)) continue;

                if (cells[x, y].candy != null)
                {
                    PoolingManager.Despawn(cells[x, y].candy.gameObject);
                    cells[x, y].candy = null;

                    ChangeScore(60);
                }
            }
        yield return new WaitForSeconds(0.1f);
    }
    IEnumerator ClearGridByColor(CandyType type)
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                var c = cells[x, y].candy;
                if (c && c.Type == type && c.SpecialType == CandySpecialType.None)
                {
                    PoolingManager.Despawn(c.gameObject);
                    cells[x, y].candy = null;

                    ChangeScore(60);
                }
            }
        yield return new WaitForSeconds(0.1f);
    }
    public void OutGame()
    {
        for(int x = 0;x <width;x++)
        {
            for(int y = 0; y< height;y++)
            {
                if (cells[x, y] != null)
                {
                    if (cells[x, y].candy != null)
                    {
                        PoolingManager.Despawn(cells[x, y].candy.gameObject);
                        cells[x, y].candy = null;
                    }

                    PoolingManager.Despawn(cells[x, y].gameObject);
                }
            }
        }
    }

    public void ChangeScore(int val)
    {
        score += val;
        ObserverManager<TextID>.PostEven(TextID.ChangeScore, score);
    }
    public void ChangeMoveLeft(int val)
    {
        movesLeft += val;
        ObserverManager<TextID>.PostEven(TextID.ChangeMoveLimit, movesLeft);
    }
    public void CheckMoveLimit()
    {
        if (score >= level.targetScore) 
        {
            gameObject.SetActive(false);
            winUI.OnWin();
            AudioController.Instance.PlayAudioGameWin();
        }
        else if(movesLeft <= 0)
        {
            gameObject.SetActive(false);
            loseUI.OnLose();
            AudioController.Instance.PlayAudioGameLose();
        }
    }
}
