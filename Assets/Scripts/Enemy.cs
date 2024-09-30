using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 스포너
    EnemySpawner _spawner;
    
    // 몬스터 스탯
    [SerializeField] private int _hp;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private int _damage;
    [SerializeField] private int _dropGold;

    // 이동 경로
    WayPoint _wayPoint;
    float rotationSpeed = 5;
    int waypointIndex = 0;
    float distanceTravelled = 0;

    public void Init(EnemySpawner spawner, EnemyStats enemyStats, WayPoint waypoint)
    {
        _spawner = spawner; // 부모 스포너
        _wayPoint = waypoint; // 웨이 포인트

        // 몬스터 스탯
        _hp = enemyStats.hp;
        _moveSpeed = enemyStats.moveSpeed;
        _damage = enemyStats.damage;
        _dropGold = enemyStats.dropGold;
    }

    private void FixedUpdate()
    {
        MoveAlongPath();
    }

    public void TakeDamage(int damage)
    {
        _hp -= damage;

        if( _hp <= 0 ) 
        {
            Dead();
        }
    }
    
    void Dead()
    {
        // 골드 드롭
        ResourceManager.Instance.AddGold(_dropGold);
        // 몬스터 풀에 다시 넣음
        _spawner.OnReleaseEnemy(this.gameObject);
    }
    
    // 길을 따라 이동
    void MoveAlongPath()
    {
        if(waypointIndex < _wayPoint.points.Length)
        {
            Vector3 targetWayPoint = _wayPoint.points[waypointIndex];
            Vector3 direction = targetWayPoint - transform.position;

            if(direction.magnitude < 0.1f)
            {
                waypointIndex++;
                ArriveNexus();   
            }
            else
            {
                transform.position += direction.normalized * _moveSpeed * Time.deltaTime;
                RotateTowards(direction);
                distanceTravelled += _moveSpeed * Time.deltaTime;
            }
        }
    }
    void ArriveNexus()
    {
        // 넥서스에 도착했을 경우
        if (waypointIndex >= _wayPoint.points.Length)
        {
            ResourceManager.Instance.TakeDamage(_damage); // 체력 감소
            _spawner.OnReleaseEnemy(this.gameObject); // 오브젝트 풀에 다시 넣기
        }
    }

    // 이동 방향으로 회전
    void RotateTowards(Vector3 direction)
    {
        if(direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    // 현재까지 이동한 거리 반환
    public float GetDistanceTravelled()
    {
        return distanceTravelled;
    }
}