using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatObject : MonoBehaviour
{
    [SerializeField]
    private float amplitude = 0.5f; // 움직임의 높이
    [SerializeField]
    private float speed = 2.0f; // 움직임의 속도
    private Vector3 startPosition;

    private void Awake()
    {
        // 시작 위치 저장
        startPosition = transform.position;
    }

    private void Update()
    {
        // 시간에 따라 y축 위치 변경
        float newY = startPosition.y + Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
