using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("카메라 이동")]
    [SerializeField] float panSpeed = 20f;             // 카메라 이동 속도
    [SerializeField] float panBorderThickness = 10f;   // 화면 끝에서 카메라가 움직이기 시작하는 거리
    [SerializeField] Vector2 panLimit;                 // 카메라 이동 제한 (x, y 축)

    [Header("카메라 줌")]
    [SerializeField] float scrollSpeed = 20f;          // 줌 속도
    [SerializeField] float minY = 20f;                 // 카메라 줌 최소값
    [SerializeField] float maxY = 120f;                // 카메라 줌 최대값

    Vector3 position;

    void Update()
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

        // 새로운 위치를 카메라에 적용
        transform.position = position;
    }
    // 카메라 줌을 처리하는 함수
    void HandleZoom()
    {
        Vector3 pos = transform.position;

        // 마우스 휠로 줌 조절
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;

        // 카메라 줌 범위 제한
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        // 새로운 위치를 카메라에 적용
        transform.position = pos;
    }
}
