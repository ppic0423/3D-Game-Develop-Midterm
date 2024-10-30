using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class PointTurret : Turret
{
    [Header("Ư�� �ɷ�")]
    [SerializeField] float damageIncrease;

    [Header("�ѱ� ��ġ")]
    [SerializeField] Vector3 muzzlePos;
    [SerializeField] private Transform startPoint;   // ������ ������ ������Ʈ
    [SerializeField] private Transform endPoint;     // ������ ���� ������Ʈ
    [SerializeField] private Transform laserBody;    // ������ ��ü
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
        if (_target == null)
        {
            ToggleLaser(false);
            return;
        }

        UpdateLaser();

        if (_target != null && _attackInterval - _synergyAttackInterval < _attackIntervalDelta)
        {
            SoundManager.Instance.PlaySound(_fireSound);

            ApplyDamageAndDebuffs(_target.GetComponent<Enemy>());
            _attackIntervalDelta = 0;
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
        // ���� ���� �ִ� ��� �ݶ��̴��� ������
        LayerMask enemyLayer = LayerMask.GetMask("Enemy");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _findRange, enemyLayer);
        float maxDistanceTravelled = float.MinValue;

        // ���� ���� ���� ���� ���
        if (hitColliders.Length == 0)
        {
            maxDistanceTravelled = 0;
            damageIncrease = 0;
            _target = null;
            return;
        }

        // �� �ݶ��̴��� ������Ʈ �̸��� ���
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