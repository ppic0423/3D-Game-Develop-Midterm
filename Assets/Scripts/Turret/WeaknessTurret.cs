using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaknessTurret : Turret
{
    [Header("Ư�� �ɷ�")]
    [SerializeField] float _damageIncreaseDuration;
    [SerializeField] float _damageIncreaseAmount;

    [Header("�ѱ� ��ġ")]
    [SerializeField] Vector3 muzzlePos;
    void Start()
    {
        Init();

        upgrades = new List<TurretUpgrade>
        {
            new TurretUpgrade { attackInterval = 0.2f, cost = 60 },
            new TurretUpgrade { attackInterval = 0.4f, cost = 120 },
        };
    }

    void Update()
    {
        Tick();
    }

    protected override void Init()
    {
        base.Init();

        debuffs.Add(new DamageInreaseDebuff(_damageIncreaseDuration, _damageIncreaseAmount));
    }

    protected override void Attack()
    {
        if (_target == null || _attackInterval - _synergyAttackInterval > _attackIntervalDelta)
            return;

        GetComponent<AudioSource>().PlayOneShot(_fireSound);

        // SoundManager.Instance.PlaySound(_fireSound);
        // ������Ʈ Ǯ���� �Ѿ� ��������
        GameObject bulletGo = CommonBulletPool.Instance.pool.Get(); 
        // �Ѿ� ������Ʈ �߰�
        bulletGo.AddComponent<WeaknessBullet>().Init(_target, _damage * _synergyDamagePlus, _bulletSpeed);

        AddDebuffOnBullet(bulletGo);

        // �Ѿ� ��ġ ����
        bulletGo.transform.position = transform.TransformPoint(muzzlePos);

        _attackIntervalDelta = 0;
    }
    public override void Upgrade()
    {
        base.Upgrade();

        TurretUpgrade upgrade = upgrades[currentLevel - 1];
        _attackInterval += upgrade.attackInterval;
    }

    protected override void FindObjectsInRange()
    {
        LayerMask enemyLayer = LayerMask.GetMask("Enemy");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _findRange, enemyLayer);

        if (hitColliders.Length == 0)
        {
            _target = null;
            return;
        }

        foreach (Collider collider in hitColliders)
        {
            if (!collider.GetComponent<Enemy>().Debuffs.OfType<DamageInreaseDebuff>().Any())
            {
                _target = collider.transform;
            }
        }
    }
}
