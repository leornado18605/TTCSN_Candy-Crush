using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Candy : MonoBehaviour
{
    [Header("Candy")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    private CandyType candyType;
    public CandyType Type => candyType;
    private CandySpecialType specialType;
    public CandySpecialType SpecialType
    {
        get => specialType;
        set => specialType = value;
    }
    public Cell CurrentCell { get; set; }

    public void Init(CandyType type , CandySpecialType specialType = CandySpecialType.None, Sprite sprite = null)
    {
        this.candyType = type;
        this.specialType = specialType;
        if(sprite != null) spriteRenderer.sprite = sprite;
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
        PoolingManager.Despawn(gameObject);
    }
}
