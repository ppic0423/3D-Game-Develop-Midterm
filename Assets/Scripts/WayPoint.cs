using UnityEngine;

public class WayPoint : MonoBehaviour
{
    [SerializeField] public Vector3[] points; // 각 이동 포인트
    [SerializeField] Color gizmoColor = Color.green; // 레이 색상

    void OnDrawGizmos()
    {
        if (points != null && points.Length > 1)
        {
            // 선의 색상을 설정
            Gizmos.color = gizmoColor;

            for (int i = 0; i < points.Length - 1; i++)
            {
                // 현재 포인트와 다음 포인트 사이에 선 그리기
                Gizmos.DrawLine(points[i], points[i + 1]);
            }
        }
    }
}