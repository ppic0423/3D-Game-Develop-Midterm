using System.Collections.Generic;
using UnityEngine;

public class SlowTurret : Turret
{
    [Header("Ư�� �ɷ�")]
    [SerializeField] Vector3 _range;
    [SerializeField] float _slowAmount;
    [SerializeField] float _slowDuration;

    [Header("�ѱ� ��ġ")]
    [SerializeField] Vector3 muzzlePos;

    SlowDebuff _slowDebuff;

    void Start()
    {
        Init();

        upgrades = new List<TurretUpgrade>
    {
        new TurretUpgrade { slowAmount = 0.1f, slowDuration = 0.25f, cost = 40 },
        new TurretUpgrade { slowAmount = 0.1f, slowDuration = 0.4f, cost = 80 },
    };

        _slowDebuff = new SlowDebuff(duration: _slowDuration, slowAmount: _slowAmount);
        debuffs.Add(_slowDebuff);
    }

    void Update()
    {
        Tick();
    }

    protected override void Init()
    {
        base.Init();
    }

    protected override void Attack()
    {
        if (_target == null || _attackInterval - _synergyAttackInterval > _attackIntervalDelta)
            return;

        GetComponent<AudioSource>().PlayOneShot(_fireSound);
        GameObject bulletGo = CommonBulletPool.Instance.pool.Get();
        GetComponent<ParticleSystem>().Play();
        bulletGo.AddComponent<SlowBullet>().Init(_target, _damage * _synergyDamagePlus, _bulletSpeed, _range);

        AddDebuffOnBullet(bulletGo);

        bulletGo.transform.position = transform.TransformPoint(muzzlePos);

        _attackIntervalDelta = 0;
    }
    public override void Upgrade()
    {
        base.Upgrade();

        TurretUpgrade upgrade = upgrades[currentLevel - 1];
        _slowAmount += upgrade.slowAmount;
        _slowDuration += upgrade.slowDuration;
    }
}
