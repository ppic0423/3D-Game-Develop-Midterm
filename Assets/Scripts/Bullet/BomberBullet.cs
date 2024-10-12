using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberBullet : Bullet
{
    Vector3 _range;

    public override void Init(Transform target, float damage, float speed)
    {
        _target = target;
        _damage = damage;
        _speed = speed;
    }
    public void Init(Transform target, float damage, float speed, Vector3 range)
    {
        Init(target, damage, speed);

        _range = range;
    }
    
    private void FixedUpdate()
    {
        MoveBullet();
    }

    protected override void HitTarget()
    {
        LayerMask targetLayer = LayerMask.GetMask("Enemy");

        MissilePool.Instance.pool.Release(this.gameObject);
        Collider[] enemyColliders = Physics.OverlapBox(transform.position, _range / 2, Quaternion.identity, targetLayer);

        MissileEffectPool.Instance.pool.Get().transform.position = _target.transform.position;
        foreach(Collider collider in enemyColliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();

            ApplyDebuffs(enemy);
            enemy.TakeDamage(_damage);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, _range);
    }
}
