using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCurrentEffect : MonoBehaviour
{
    public float amplitude = 10f;   // độ cao nhún (px hoặc unit)
    public float frequency = 2f;    // tốc độ nhún

    private Vector3 startPos;

    void Start()
    {
        // Lưu vị trí gốc để nhún quanh nó
        startPos = transform.localPosition;
    }

    void Update()
    {
        // Tính offset Y theo sin
        float offsetY = Mathf.Sin(Time.time * frequency) * amplitude;

        // Gán vị trí mới (chỉ đổi Y, giữ nguyên X/Z)
        transform.localPosition = startPos + new Vector3(0, offsetY, 0);
    }

    public void SetPos(Vector3 pos)
    {
        pos.y += 178;
        transform.localPosition = pos;
    }
}
