using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("ī�޶� �̵�")]
    [SerializeField] float panSpeed = 20f;             // ī�޶� �̵� �ӵ�
    [SerializeField] float panBorderThickness = 10f;   // ȭ�� ������ ī�޶� �����̱� �����ϴ� �Ÿ�
    [SerializeField] Vector2 panLimit;                 // ī�޶� �̵� ���� (x, y ��)

    [Header("ī�޶� ��")]
    [SerializeField] float scrollSpeed = 20f;          // �� �ӵ�
    [SerializeField] float minView = 20f;                 // ī�޶� �� �ּҰ�
    [SerializeField] float maxView = 120f;                // ī�޶� �� �ִ밪

    Vector3 position;

    void Start()
    {
        position = transform.position;
    }
    private void FixedUpdate()
    {
        HandleMovement();  // ī�޶� �̵� ó��
        HandleZoom();      // ī�޶� �� ó��
    }

    // ī�޶� �̵��� ó���ϴ� �Լ�
    void HandleMovement()
    {
        // ���� y ��ġ�� ����
        float currentY = position.y;

        // ���콺 �Ǵ� Ű����� ī�޶� �̵�
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

        // ī�޶� �̵� ���� ����
        position.x = Mathf.Clamp(position.x, -panLimit.x, panLimit.x);
        position.z = Mathf.Clamp(position.z, -panLimit.y, panLimit.y);

        // y�� ��ġ�� ����
        position.y = currentY;

        // ���ο� ��ġ�� ī�޶� ����
        transform.position = position;
    }
    // ī�޶� ���� ó���ϴ� �Լ�
    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.fieldOfView -= scroll * scrollSpeed * Time.deltaTime;
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, minView, maxView);
    }
}