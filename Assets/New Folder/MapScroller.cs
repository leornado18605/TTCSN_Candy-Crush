using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if DOTWEEN
using DG.Tweening;
#endif

[DisallowMultipleComponent]
public class MapScroller : MonoBehaviour
{
    // ───────────────────────── Config chung ─────────────────────────
    [Header("Content to move (Map root)")]
    public Transform content;                // node gốc của map (mọi thứ là con)

    [Header("Camera")]
    public Camera cam;                       // để tính tọa độ & biên
    [Tooltip("Khóa cuộn theo trục dọc (Candy Crush style).")]
    public bool verticalOnly = true;

    // ───────────────────────── Kéo & Quán tính ─────────────────────
    [Header("Drag")]
    [Tooltip("Độ nhạy kéo (world units).")]
    public float dragMultiplier = 1.0f;

    [Header("Inertia")]
    public bool enableInertia = true;
    public float decel = 6f;                 // m/s^2
    public float maxSpeed = 30f;             // m/s

    // ───────────────────────── Biên cuộn (tự tính) ─────────────────
    [Header("Auto bounds from sprite")]
    [Tooltip("Sprite nền (nằm dưới 'content'). Dùng để tự tính min/maxBound.")]
    public SpriteRenderer mapSprite;
    public bool autoComputeBounds = true;
    public Vector2 boundsPadding = new Vector2(0.1f, 0.1f); // chừa mép (world)

    [Header("Manual bounds (fallback)")]
    public Vector2 minBound = new Vector2(-5f, -20f);
    public Vector2 maxBound = new Vector2(5f, 80f);

    // ───────────────────────── Snap vào Level ───────────────────────
    [Header("Snap to level")]
    public bool snapOnRelease = true;
    public float snapThreshold = 1.2f;       // khoảng cách bắt dính (world)
    public float snapDuration = 0.35f;
    public AnimationCurve snapEase = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public List<Transform> levelAnchors = new List<Transform>();

    // ───────────────────────── Parallax ─────────────────────────────
    [System.Serializable] public class ParallaxLayer { public Transform t; [Range(0f, 1f)] public float factor = 0.3f; }
    [Header("Parallax")]
    public List<ParallaxLayer> parallaxLayers = new List<ParallaxLayer>();

    [Header("Misc")]
    public bool blockWhileSnapping = true;

#if DOTWEEN
    [Header("DOTween (optional)")]
    public bool useDOTWEEN = true;
#endif

    // ───────────────────────── Internal ─────────────────────────────
    Vector3 lastPointerWorld;
    Vector2 velocity;            // m/s (world)
    bool dragging;
    bool snapping;

    void Reset()
    {
        cam = Camera.main;
    }

    void Awake()
    {
        if (!cam) cam = Camera.main;
        if (!content) Debug.LogError("[MapScroller] Missing 'content' (MapRoot).");
    }

    void Start()
    {
        if (autoComputeBounds) RecomputeBounds();
    }

    void OnValidate()
    {
        // Khi chỉnh trong Editor (chưa Play), thử tính lại biên để xem trước
        if (!Application.isPlaying && autoComputeBounds)
        {
            if (!cam) cam = Camera.main;
            TryRecomputeInEditor();
        }
    }

    void Update()
    {
        if (snapping && blockWhileSnapping) return;

        // ── Input: mouse/touch
        bool down = Input.GetMouseButtonDown(0);
        bool held = Input.GetMouseButton(0);
        bool up = Input.GetMouseButtonUp(0);

        if (Input.touchSupported && Input.touchCount > 0)
        {
            var t = Input.GetTouch(0);
            down = t.phase == TouchPhase.Began;
            held = t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary;
            up = t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled;
        }

        if (down)
        {
            dragging = true;
            lastPointerWorld = ScreenToWorld(Input.mousePosition);
            velocity = Vector2.zero;
        }
        else if (held && dragging)
        {
            Vector3 cur = ScreenToWorld(Input.mousePosition);
            Vector3 delta = cur - lastPointerWorld;
            if (verticalOnly) delta.x = 0f;

            MoveContent(delta * dragMultiplier);

            // cập nhật vận tốc (m/s)
            velocity = Vector2.ClampMagnitude(
                (Vector2)delta / Mathf.Max(Time.deltaTime, 1e-5f),
                maxSpeed
            );

            lastPointerWorld = cur;
        }
        else if (up && dragging)
        {
            dragging = false;
            if (enableInertia) StartCoroutine(InertiaCoroutine());
            else if (snapOnRelease) TrySnap();
        }
    }

    // ───────────────────────── Movement & Clamp ─────────────────────
    void MoveContent(Vector2 delta)
    {
        if (!content) return;

        content.position += new Vector3(delta.x, delta.y, 0f);

        // clamp theo bounds
        Vector3 p = content.position;
        p.x = Mathf.Clamp(p.x, minBound.x, maxBound.x);
        p.y = Mathf.Clamp(p.y, minBound.y, maxBound.y);
        content.position = p;

        // Parallax: layer xa di chuyển ít hơn
        if (parallaxLayers.Count > 0)
        {
            foreach (var layer in parallaxLayers)
            {
                if (!layer.t) continue;
                layer.t.position -= new Vector3(delta.x, delta.y, 0f) * (1f - layer.factor);
            }
        }
    }

    IEnumerator InertiaCoroutine()
    {
        float vMag = velocity.magnitude;
        Vector2 vDir = velocity.normalized;

        while (vMag > 0.05f && !dragging)
        {
            float dt = Time.deltaTime;
            Vector2 step = vDir * vMag * dt;
            MoveContent(step);
            vMag = Mathf.Max(0f, vMag - decel * dt);
            yield return null;
        }

        velocity = Vector2.zero;
        if (snapOnRelease) TrySnap();
    }

    // ───────────────────────── Snap ─────────────────────────────────
    void TrySnap()
    {
        if (levelAnchors == null || levelAnchors.Count == 0) return;

        Transform best = null;
        float bestD = float.MaxValue;
        Vector3 cPos = content.position;

        // tìm mốc gần nhất (theo trục dọc nếu verticalOnly)
        for (int i = 0; i < levelAnchors.Count; i++)
        {
            var t = levelAnchors[i];
            if (!t) continue;

            float d = verticalOnly
                ? Mathf.Abs(cPos.y - t.position.y)
                : Vector2.Distance(new Vector2(cPos.x, cPos.y), new Vector2(t.position.x, t.position.y));

            if (d < bestD) { bestD = d; best = t; }
        }

        if (best && bestD <= snapThreshold)
        {
            Vector3 target = verticalOnly
                ? new Vector3(cPos.x, best.position.y, cPos.z)
                : new Vector3(best.position.x, best.position.y, cPos.z);

            StartCoroutine(SnapTo(target));
        }
    }

    IEnumerator SnapTo(Vector3 worldTarget)
    {
        snapping = true;
#if DOTWEEN
        if (useDOTWEEN)
        {
            if (verticalOnly)
                content.DOMoveY(worldTarget.y, snapDuration).SetEase(snapEase);
            else
                content.DOMove(new Vector3(worldTarget.x, worldTarget.y, content.position.z), snapDuration).SetEase(snapEase);
            yield return new WaitForSeconds(snapDuration);
        }
        else
#endif
        {
            float t = 0f;
            Vector3 start = content.position;
            while (t < 1f)
            {
                t += Time.deltaTime / Mathf.Max(0.0001f, snapDuration);
                float e = snapEase.Evaluate(Mathf.Clamp01(t));
                content.position = Vector3.Lerp(start, worldTarget, e);
                yield return null;
            }
            content.position = worldTarget;
        }
        snapping = false;
    }

    // ───────────────────────── Bounds helper ────────────────────────
    /// <summary>
    /// Tính min/maxBound sao cho mép sprite luôn còn trong khung nhìn của camera orthographic.
    /// Gọi lại hàm này khi đổi ảnh nền, scale, tỉ lệ màn, hoặc orthographicSize.
    /// </summary>
    public void RecomputeBounds()
    {
        if (!content || !cam) return;

        if (mapSprite == null)
        {
            Debug.LogWarning("[MapScroller] mapSprite chưa gán – dùng min/maxBound thủ công.");
            return;
        }

        // Kích thước khung nhìn (world units)
        float halfH = cam.orthographicSize;
        float halfW = halfH * cam.aspect;
        Vector2 halfView = new Vector2(halfW, halfH);

        // Kích thước sprite (world) – sprite là con của 'content'
        Bounds sb = mapSprite.bounds; // world bounds của sprite
        Vector2 spriteExt = (Vector2)sb.extents;

        // Nếu sprite nhỏ hơn khung nhìn => không cho kéo (giữ giữa)
        Vector2 slack = spriteExt - halfView - boundsPadding;
        slack.x = Mathf.Max(0, slack.x);
        slack.y = Mathf.Max(0, slack.y);

        // camera đứng yên, dịch 'content' quanh vị trí camera
        Vector3 camPos = cam.transform.position;

        minBound = new Vector2(camPos.x - slack.x, camPos.y - slack.y);
        maxBound = new Vector2(camPos.x + slack.x, camPos.y + slack.y);
    }

    // gọi từ OnValidate khi ở Editor để xem trước (không spam warning)
    void TryRecomputeInEditor()
    {
        if (content && cam && mapSprite)
        {
            float halfH = cam.orthographicSize;
            float halfW = halfH * cam.aspect;
            Vector2 halfView = new Vector2(halfW, halfH);
            Bounds sb = mapSprite.bounds;
            Vector2 spriteExt = (Vector2)sb.extents;
            Vector2 slack = spriteExt - halfView - boundsPadding;
            slack.x = Mathf.Max(0, slack.x);
            slack.y = Mathf.Max(0, slack.y);
            Vector3 camPos = (cam ? cam.transform.position : Vector3.zero);
            minBound = new Vector2(camPos.x - slack.x, camPos.y - slack.y);
            maxBound = new Vector2(camPos.x + slack.x, camPos.y + slack.y);
        }
    }

    // ───────────────────────── Utility ──────────────────────────────
    Vector3 ScreenToWorld(Vector3 screenPos)
    {
        if (!cam) cam = Camera.main;

        // Tính khoảng cách từ camera đến content để Convert đúng
        float z = Mathf.Abs(cam.transform.position.z - content.position.z);
        return cam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, z));
    }
}
