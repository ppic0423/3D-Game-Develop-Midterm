using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidTurret : Turret
{
    [Header("ÃÑ±¸ À§Ä¡")]
    [SerializeField] Vector3 leftMuzzlePos;
    [SerializeField] Vector3 rightMuzzlePos;

    bool isLeftFire = false;
    private void Start()
    {
        Init();

        upgrades = new List<TurretUpgrade>
        {
            new TurretUpgrade { damage = 1, attackInterval = 0.3f ,cost = 33 },
            new TurretUpgrade { damage = 3, attackInterval = 0.6f, cost = 66 },
        };
    }

    private void Update()
    {
        Tick();
    }

    protected override void Attack()
    {
        if (_target == null || _attackInterval - _synergyAttackInterval > _attackIntervalDelta)
            return;

        GameObject bulletGo = CommonBulletPool.Instance.pool.Get();
        bulletGo.AddComponent<CommonBullet>().Init(_target, _damage * _synergyDamagePlus, _bulletSpeed);

        AddDebuffOnBullet(bulletGo);

        bulletGo.transform.position = transform.TransformPoint(isLeftFire ? leftMuzzlePos : rightMuzzlePos);
        isLeftFire = !isLeftFire;

        _attackIntervalDelta = 0;
    }
    public override void Upgrade()
    {
        base.Upgrade();

        TurretUpgrade upgrade = upgrades[currentLevel - 1];

        _attackInterval -= upgrade.attackInterval;
    }
}
