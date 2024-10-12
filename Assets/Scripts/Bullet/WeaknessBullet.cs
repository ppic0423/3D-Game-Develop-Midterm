using UnityEngine;

public class WeaknessBullet : Bullet
{
    [Header("총알 스탯")]
    [SerializeField] float _damageIncreaseDuration;
    [SerializeField] float _damageIncreaseAmount;
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
        CommonBulletPool.Instance.pool.Release(this.gameObject);
        ApplyDebuffs(_target.GetComponent<Enemy>());
    }
}
