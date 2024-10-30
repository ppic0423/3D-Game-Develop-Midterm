using System.Collections.Generic;
using UnityEngine;

public class BomberTurret : Turret
{
    [Header("특수 능력")]
    [SerializeField] Vector3 _range;

    [Header("총구 위치")]
    [SerializeField] Vector3 muzzlePos;

    void Start()
    {
        Init();

        upgrades = new List<TurretUpgrade>
        {
            new TurretUpgrade { damage = 3, rangeIncrease = 1, cost = 40 },
            new TurretUpgrade { damage = 7, rangeIncrease = 2, cost = 80 },
        };
    }
    void Update()
    {
        Tick();
    }

    protected override void Attack()
    {
        if (_target == null || _attackInterval - _synergyAttackInterval > _attackIntervalDelta)
            return;

        GameObject bulletGo = MissilePool.Instance.pool.Get();
        bulletGo.AddComponent<BomberBullet>().Init(_target, _damage * _synergyDamagePlus, _bulletSpeed, _range);
        bulletGo.transform.position = transform.TransformPoint(muzzlePos);
        GetComponent<AudioSource>().PlayOneShot(_fireSound);

        AddDebuffOnBullet(bulletGo);

        _attackIntervalDelta = 0;
    }
    public override void Upgrade()
    {
        base.Upgrade();

        TurretUpgrade upgrade = upgrades[currentLevel - 1];

        _damage += upgrade.damage;
        _findRange += upgrade.rangeIncrease;
    }
}
