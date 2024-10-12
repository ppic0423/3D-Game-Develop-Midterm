using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineBullet : Bullet
{
    [HideInInspector] public List<MineBullet> m_List;
    float _radius;
    WayPoint wayPoint;
    Vector3 targetPos = Vector3.zero;

    public override void Init(Transform target, float damage, float speed)
    {
        _target = null;
        _damage = damage;
        _speed = speed;
    }
    public void Init(Transform target, float damage, float speed, float radius)
    {
        Init(target, damage, speed);

        _radius = radius;
        wayPoint = FindObjectOfType<WayPoint>();
        targetPos = GetClosestPointOnPath();
    }
    void FixedUpdate()
    {
        MoveBullet();
    }

    protected override void HitTarget()
    {
        MinePool.Instance.pool.Release(this.gameObject);
        m_List.Remove(this);
    }

    protected override void MoveBullet()
    {
        // Áö·Ú ÀÌµ¿
        if (Vector3.Distance(transform.position, targetPos) > 0.25f)
        {
            Vector3 direction = (targetPos - transform.position).normalized;
            transform.position += direction * _speed * Time.deltaTime;
        }
        else
        {
            transform.position = targetPos;
        }

        // Áö·Ú Æø¹ß
        LayerMask targetLayer = LayerMask.GetMask("Enemy");
        Collider[] enemyColliders = Physics.OverlapSphere(transform.position, _radius, targetLayer);
        if (enemyColliders.Length > 0)
        {
            HitTarget();
            foreach(Collider collider in enemyColliders)
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                ApplyDebuffs(enemy);
                enemy.TakeDamage(_damage);
            }
        }
    }
    Vector3 GetClosestPointOnPath()
    {
        Vector3 closestPoint = Vector3.zero;
        float closestDistance = float.MaxValue;
        float randomValue = Random.Range(0f, 1f);

        for (int i = 0; i < wayPoint.points.Length - 1; i++)
        {
            Vector3 pointA = wayPoint.points[i];
            Vector3 pointB = wayPoint.points[i + 1];

            Vector3 randomTransform = new Vector3(transform.position.x + randomValue, transform.position.y, transform.position.z + randomValue);
            Vector3 closestPointOnSegment = GetClosestPointOnSegment(pointA, pointB, randomTransform);
            float distance = Vector3.Distance(transform.position, closestPointOnSegment);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = closestPointOnSegment;
            }
        }

        return closestPoint;
    }
    Vector3 GetClosestPointOnSegment(Vector3 pointA, Vector3 pointB, Vector3 currentPosition)
    {
        Vector3 AB = pointB - pointA;
        Vector3 AC = currentPosition - pointA;

        float t = Vector3.Dot(AC, AB) / Vector3.Dot(AB, AB);
        t = Mathf.Clamp01(t);

        return pointA + t * AB;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, _radius);
    }
}
