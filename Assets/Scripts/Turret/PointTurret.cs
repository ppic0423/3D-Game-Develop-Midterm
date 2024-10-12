using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class PointTurret : Turret
{
    [Header("특수 능력")]
    [SerializeField] float damageIncrease;

    [Header("총구 위치")]
    [SerializeField] Vector3 muzzlePos;
    [SerializeField] private Transform startPoint;   // 레이저 시작점 오브젝트
    [SerializeField] private Transform endPoint;     // 레이저 끝점 오브젝트
    [SerializeField] private Transform laserBody;    // 레이저 본체
    void Start()
    {
        Init();

        upgrades = new List<TurretUpgrade>
        {
            new TurretUpgrade { attackInterval = 0.25f, damageIncrease = 0, cost = 65 },
            new TurretUpgrade { attackInterval = 0.5f, damageIncrease = 0.3f,cost = 135 },
        };
    }
    void Update()
    {
        Tick();
    }

    protected override void Attack()
    {
        /*if (_target == null || _attackInterval - _synergyAttackInterval > _attackIntervalDelta)
            return;

        GameObject bulletGo = BulletPool.Instance.pool.Get();

        bulletGo.AddComponent<CommonBullet>().Init(_target, (_damage + damageIncrease) * _synergyDamagePlus, _bulletSpeed);

        AddDebuffOnBullet(bulletGo);

        bulletGo.transform.position = transform.TransformPoint(muzzlePos);

        _attackIntervalDelta = 0;*/

        if (_target == null || _attackInterval - _synergyAttackInterval > _attackIntervalDelta)
        {
            ToggleLaser(false);
            return;
        }

        UpdateLaser();

        if (_target != null)
        {
            ApplyDamageAndDebuffs(_target.GetComponent<Enemy>());
        }
    }
    private void ApplyDamageAndDebuffs(Enemy enemy)
    {
        enemy.TakeDamage(_damage * _synergyDamagePlus);
        foreach (Debuff debuff in debuffs)
        {
            enemy.ApplyDebuff(debuff);
        }
    }
    private void ToggleLaser(bool isActive)
    {
        startPoint.gameObject.SetActive(isActive);
        endPoint.gameObject.SetActive(isActive);
        laserBody.gameObject.SetActive(isActive);
    }
    private void UpdateLaser()
    {
        Vector3 startPosition = startPoint.position;
        Vector3 endPosition = _target.position;

        endPoint.position = endPosition;
        UpdateLaserBody(startPosition, endPosition);

        ToggleLaser(true);
    }
    private void UpdateLaserBody(Vector3 startPosition, Vector3 endPosition)
    {
        Vector3 midPoint = (startPosition + endPosition) / 2;
        laserBody.position = midPoint;

        float distance = Vector3.Distance(startPosition, endPosition);
        laserBody.localScale = new Vector3(laserBody.localScale.x, distance / 2, laserBody.localScale.z);

        Vector3 direction = (endPosition - startPosition).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90f, 0f, 0f);
        laserBody.rotation = rotation;
    }


    public override void Upgrade()
    {
        base.Upgrade();

        TurretUpgrade upgrade = upgrades[currentLevel - 1];

        _attackInterval -= upgrade.attackInterval;
        damageIncrease += upgrade.damageIncrease;
    }
    protected override void FindObjectsInRange()
    {
        // 범위 내에 있는 모든 콜라이더를 가져옴
        LayerMask enemyLayer = LayerMask.GetMask("Enemy");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _findRange, enemyLayer);
        float maxDistanceTravelled = float.MinValue;

        // 범위 내에 적이 없을 경우
        if (hitColliders.Length == 0)
        {
            maxDistanceTravelled = 0;
            damageIncrease = 0;
            _target = null;
            return;
        }

        // 각 콜라이더의 오브젝트 이름을 출력
        foreach (Collider collider in hitColliders)
        {
            if (collider.GetComponent<Enemy>().GetDistanceTravelled() > maxDistanceTravelled)
            {
                _target = collider.transform;
                maxDistanceTravelled = collider.GetComponent<Enemy>().GetDistanceTravelled();
            }
        }
    }
}