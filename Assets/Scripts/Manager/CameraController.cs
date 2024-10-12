using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class CameraController : MonoBehaviour
{
    [Header("카메라 이동")]
    [SerializeField] float panSpeed = 20f;             // 카메라 이동 속도
    [SerializeField] float panBorderThickness = 10f;   // 화면 끝에서 카메라가 움직이기 시작하는 거리
    [SerializeField] Vector2 panLimit;                 // 카메라 이동 제한 (x, y 축)

    [Header("카메라 줌")]
    [SerializeField] float scrollSpeed = 20f;          // 줌 속도
    [SerializeField] float minView = 20f;                 // 카메라 줌 최소값
    [SerializeField] float maxView = 120f;                // 카메라 줌 최대값

    Vector3 position;

    void Start()
    {
        position = transform.position;
    }
    private void FixedUpdate()
    {
        HandleMovement();  // 카메라 이동 처리
        HandleZoom();      // 카메라 줌 처리
    }

    // 카메라 이동을 처리하는 함수
    void HandleMovement()
    {
        // 현재 y 위치를 저장
        float currentY = position.y;

        // 마우스 또는 키보드로 카메라 이동
        if (Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            position.z += panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y <= panBorderThickness)
        {
            position.z -= panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            position.x += panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x <= panBorderThickness)
        {
            position.x -= panSpeed * Time.deltaTime;
        }

        // 카메라 이동 범위 제한
        position.x = Mathf.Clamp(position.x, -panLimit.x, panLimit.x);
        position.z = Mathf.Clamp(position.z, -panLimit.y, panLimit.y);

        // y축 위치를 고정
        position.y = currentY;

        // 새로운 위치를 카메라에 적용
        transform.position = position;
    }
    // 카메라 줌을 처리하는 함수
    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.fieldOfView -= scroll * scrollSpeed * Time.deltaTime;
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, minView, maxView);
    }
}