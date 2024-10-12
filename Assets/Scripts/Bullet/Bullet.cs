using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    protected Transform _target;
    protected float _damage;
    protected float _speed = 0;

    protected float _saveTime = 0;

    protected List<Debuff> debuffs = new List<Debuff> ();

    public virtual void Init(Transform target, float damage, float speed)
    {
    }

    protected virtual void MoveBullet()
    {
        // 생존 시간 체크
        _saveTime += Time.deltaTime;
        // 타겟이 없거나 생존 시간이 5초 이상일 경우 제거
        if (_target == null && _saveTime >= 5.0f)
        {
            CommonBulletPool.Instance.pool.Release(this.gameObject);
            return;
        }

        Vector3 direction = (_target.position - transform.position).normalized;
        transform.position += direction * _speed * Time.deltaTime;
        transform.LookAt(_target);

        if (Vector3.Distance(transform.position, _target.transform.position) < 0.5f)
        {
            HitTarget();
        }
    }
    protected virtual void HitTarget()
    {
        CommonBulletPool.Instance.pool.Release(this.gameObject);
    }

    public void AddDebuffs(Debuff debuff)
    {
        debuffs.Add(debuff);
    }
    public void ApplyDebuffs(Enemy enemy)
    {
        foreach (Debuff debuff in debuffs)
        {
            debuff.enemy = enemy;
            enemy.ApplyDebuff(debuff);
        }
    }
}
