using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatlingTurret : Turret
{
    [Header("특수 능력")]
    [SerializeField] int fireCount;
    [SerializeField] float restTime;
    float restTimeDelta;

    [Header("총구 위치")]
    [SerializeField] Vector3 muzzlePos;

    void Start()
    {
        restTimeDelta = 0;
        Init();

        upgrades = new List<TurretUpgrade>
        {
            new TurretUpgrade { damage = 2, restTime = 0.3f, cost = 130 },
            new TurretUpgrade { damage = 4, restTime = 0.6f, cost = 260 },
        };
    }

    void Update()
    {
        Tick();
        restTimeDelta -= Time.deltaTime;
    }

    protected override void Attack()
    {
        if (_target == null || _attackInterval - _synergyAttackInterval > _attackIntervalDelta || restTimeDelta > 0)
            return;

        GameObject bulletGo = CommonBulletPool.Instance.pool.Get();
        bulletGo.AddComponent<CommonBullet>().Init(_target, _damage * _synergyDamagePlus, _bulletSpeed);
        bulletGo.transform.position = transform.TransformPoint(muzzlePos);

        AddDebuffOnBullet(bulletGo);
        GetComponent<AudioSource>().PlayOneShot(_fireSound);
        _attackIntervalDelta = 0;

        // 휴식 시간 초기화
        fireCount--;
        if (fireCount == 0)
        {
            restTimeDelta = restTime;
            fireCount = 20;
        }
    }
    public override void Upgrade()
    {
        base.Upgrade();

        TurretUpgrade upgrade = upgrades[currentLevel - 1];

        _damage += upgrade.damage;
        restTime -= upgrade.restTime;
    }
}
