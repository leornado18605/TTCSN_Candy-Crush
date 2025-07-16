using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelAnimation : MonoBehaviour
{
    [SerializeField] float swingAngle;
    [SerializeField] float swingSpeed;
    [SerializeField] float swingEndSpeed;
    [SerializeField] float oscillationTime;
    private Quaternion startAngle;
    private Quaternion endAngle;
    private Quaternion blancedPoint;
    private Quaternion currentAngle;
    private Quaternion Angle;
    private float timeWave;
    private float returnTimer = 0f;
    private bool isReturning = false;

    void Start()
    {
        startAngle = Quaternion.Euler(0, 0, swingAngle);
        endAngle = Quaternion.Euler(0, 0, -swingAngle);
        blancedPoint = Quaternion.Euler(0, 0, 0);
    }

    void Update()
    {
        if (Time.time <= oscillationTime)
        {
            timeWave = Mathf.PingPong(Time.time * swingSpeed, 1);
            currentAngle = Quaternion.Lerp(startAngle, endAngle, timeWave);
            Angle = currentAngle;
        }
        else
        {
            if (!isReturning)
        {
            // Bắt đầu quá trình quay về
            isReturning = true;
            returnTimer = 0f;
        }

        returnTimer += Time.deltaTime;
        float t = Mathf.Clamp01(returnTimer * swingEndSpeed); // tăng từ 0 đến 1

        Angle = Quaternion.Lerp(currentAngle, blancedPoint, t);
        }

        transform.rotation = Angle;
       
    }
}

