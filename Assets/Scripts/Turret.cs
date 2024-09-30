using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float findRange = 5f;  // 탐색 범위
    public Transform center;  // 탐색 중심 위치

    public GameObject target;
    void Start()
    {
        if(center == null)
        {
            center = GetComponent<Transform>();
        }
    }

    private void Update()
    {
        Attack();
    }

    void Attack()
    {
        if (target != null)
        {

        }
    }

    void FindObjectsInRange()
    {
        // 범위 내에 있는 모든 콜라이더를 가져옴
        Collider[] hitColliders = Physics.OverlapSphere(center.position, findRange, (int)Define.Layer.Enemy);

        // 각 콜라이더의 오브젝트 이름을 출력
        foreach (Collider collider in hitColliders)
        {
            float compareDistance = collider.gameObject.GetComponent<Enemy>().GetDistanceTravelled();

            // 타겟이 없거나 현재 타겟보다 더 멀리갔을 경우
            if (target == null || compareDistance > target.GetComponent<Enemy>().GetDistanceTravelled())
            {
                target = collider.gameObject;
            }
        }
    }

    void OnDrawGizmos()
    {
        // 편집기에서 시각적으로 범위를 확인하기 위해 그리기
        if (center != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(center.position, findRange);
        }
    }
}
