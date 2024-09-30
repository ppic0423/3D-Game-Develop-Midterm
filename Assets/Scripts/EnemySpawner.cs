using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] BattlePhase battlePhase;

    [Header("스폰 관련")]
    [SerializeField] float spawnInterval = 1f; // 소환 시간 텀
    [SerializeField] int maxSpawnCount = 10; // 최대 소환 개수
    private int spawnCountDelta = 0;

    ObjectPool<GameObject> pool; // 오브젝트 풀
    int deadEnemyCount = 0;
    [SerializeField] int stage;
    
    private void Start()
    {
        pool = new ObjectPool<GameObject>(
            createFunc: CreateEnemy,
            actionOnGet: OnGetEnemy,
            actionOnRelease: OnReleaseEnemy,
            actionOnDestroy: DestroyEnemy,
            defaultCapacity: 10
            );
    }
    
    public void Init()
    {
        // 변수 초기화
        spawnCountDelta = maxSpawnCount;
        deadEnemyCount = 0;

        // TODO. Change Enemy Stats
        currentEnemyStat = enemyStats[stage % enemyStats.Count];

        // 적 소환 시작
        InvokeRepeating(nameof(SpawnEnemy), 0f, spawnInterval);
    }

    private void SpawnEnemy()
    {
        if (spawnCountDelta > 0)
        {
            pool.Get();
        }
    }

    #region ObjectPool
    [Header("세팅 관련")]
    [SerializeField] GameObject enemyPrefab; // 몬스터 프리팹
    [SerializeField] WayPoint wayPoint; // 이동 경로
    [SerializeField] List<EnemyStats> enemyStats; // 몬스터 순서
    EnemyStats currentEnemyStat;
    private GameObject CreateEnemy()
    {
        GameObject monster = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        monster.gameObject.transform.parent = this.transform;
        monster.SetActive(false);
        return monster;
    }
    private void OnGetEnemy(GameObject enemy)
    {
        // TODO. 적의 능력치 및 모델링 변경하기
        enemy.GetComponent<Enemy>().Init(this, currentEnemyStat ,wayPoint);

        spawnCountDelta--; // 스폰 카운트 감소
        enemy.SetActive(true); // 적 활성화
    }
    public void OnReleaseEnemy(GameObject enemy)
    {
        enemy.SetActive(false); // 적 비활성화
        deadEnemyCount++;

        // 모든 적이 사망한 경우
        if(deadEnemyCount == maxSpawnCount)
        {
            CancelInvoke(nameof(SpawnEnemy));
            battlePhase.ChangePhase();
        }
    }
    void DestroyEnemy(GameObject enemy)
    {
        Destroy(enemy);
    }
    #endregion
}
