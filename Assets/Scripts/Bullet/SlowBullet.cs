using UnityEngine;

public class SlowBullet : Bullet
{
    Vector3 _boxSize;
    float _slowDuration;
    float _slowAmount;
    public override void Init(Transform target, float damage, float speed)
    {
        _target = target;
        _damage = damage;
        _speed = speed;
    }
    public void Init(Transform target, float damage, float speed, Vector3 range)
    {
        Init(target, damage, speed);
        _boxSize = range;
    }
    private void FixedUpdate()
    {
        MoveBullet();
    }
    protected override void HitTarget()
    {
        CommonBulletPool.Instance.pool.Release(this.gameObject); // 오브젝트 풀로 반환

        // 주변 적 탐색
        LayerMask targetLayer = LayerMask.GetMask("Enemy");
        Collider[] enemyColliders = Physics.OverlapBox(transform.position, _boxSize / 2, Quaternion.identity, targetLayer);

        // 적에게 디버프 부여
        foreach (Collider collider in enemyColliders)
        {
            ApplyDebuffs(collider.GetComponent<Enemy>());
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, _boxSize);
    }
}
