using UnityEngine;
using UnityEngine.EventSystems;

public class DragMap : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private float velocityY;
    private bool dragging;

    [SerializeField] private float inertia = 0.95f; // 0.9 = dừng nhanh, 0.99 = trôi lâu

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragging = true;
        velocityY = 0f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // chỉ cộng delta Y
        Vector2 pos = rectTransform.anchoredPosition;
        pos.y += eventData.delta.y;
        rectTransform.anchoredPosition = pos;

        // lưu vận tốc Y
        velocityY = eventData.delta.y / Time.deltaTime;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
    }

    void Update()
    {
        if (!dragging && Mathf.Abs(velocityY) > 0.1f)
        {
            // cập nhật vị trí theo vận tốc
            Vector2 pos = rectTransform.anchoredPosition;
            pos.y += velocityY * Time.deltaTime;
            if(pos.y > 0) pos.y = 0; // giới hạn không kéo xuống dưới
            if(pos.y < -6150) pos.y = -6150; // giới hạn không kéo lên trên quá
            rectTransform.anchoredPosition = pos;

            // giảm dần vận tốc
            velocityY *= inertia;
        }
    }
}
