using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonBullet : Bullet
{
    public override void Init(Transform target, float damage, float speed)
    {
        _target = target;
        _damage = damage;
        _speed = speed;
    }
    private void FixedUpdate()
    {
        MoveBullet();
    }

    protected override void HitTarget()
    {
        base.HitTarget();
        Enemy enemy = _target.GetComponent<Enemy>();

        ApplyDebuffs(enemy);
        enemy.TakeDamage(_damage);
    }
}
