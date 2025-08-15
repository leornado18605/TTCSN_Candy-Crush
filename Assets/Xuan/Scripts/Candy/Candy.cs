using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Candy : MonoBehaviour
{
    [Header("Candy")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject prefabs;
    private CandyType candyType;
    public CandyType Type => candyType;
    private CandySpecialType specialType;
    public CandySpecialType SpecialType
    {
        get => specialType;
        set => specialType = value;
    }
    public GameObject Prefabs => prefabs;
    public Cell CurrentCell { get; set; }
    public int row;
    public int column;

    public void Init(CandyType type , CandySpecialType specialType = CandySpecialType.None)
    {
        this.candyType = type;
        this.specialType = specialType;
    }
    public void SetPosition(int r, int c)
    {
        row = r;
        column = c;
        
    }

    public void CandyPop()
    {
        PoolingManager.Despawn(gameObject);
    }

    public IEnumerator AnimateMoveTo(Vector3 worldTarget, float duration)
    {
        Vector3 start = transform.position;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / duration);
            transform.position = Vector3.Lerp(start, worldTarget, k);
            yield return null;
        }
        transform.position = worldTarget;
    }

    public IEnumerator AnimatePop(float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            transform.Rotate(0, 0, 720f * Time.deltaTime);
            yield return null;
        }
        Destroy(gameObject);
    }
}
