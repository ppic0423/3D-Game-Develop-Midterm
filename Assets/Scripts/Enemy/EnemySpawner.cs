using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] BattlePhase battlePhase;

    [Header("스폰 관련")]
    [SerializeField] float spawnInterval = 1f; // 소환 시간 텀
    [SerializeField] int maxSpawnCount = 10; // 최대 소환 개수
    [SerializeField] TextMeshProUGUI waveText;
    [SerializeField] TextMeshProUGUI remainEnemyText;
    private int spawnCountDelta = 0;

    int deadEnemyCount = 0;
    int stage = 1;
    int cycle = 1;

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
        remainEnemyText.text = maxSpawnCount.ToString();
        waveText.text = $"Wave {stage.ToString()}";

        // 사이클을 전부 돌았을 경우
        if (stage == enemyStats.Count)
        {
            cycle++;
            switch (cycle)
            {
                case 1:
                    enemyStatWeightDelta = enemyStatWeight;
                    break;
                case 2:
                    enemyStatWeightDelta = enemyStatWeight * enemyStatWeight;
                    break;
            }
        }
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
    ObjectPool<GameObject> pool; // 오브젝트 풀

    [Header("세팅 관련")]
    [SerializeField] GameObject enemyPrefab; // 기본 몬스터 프리팹
    [SerializeField] List<GameObject> enemyModels; // 스테이지별 몬스터 모델 리스트
    [SerializeField] WayPoint wayPoint; // 이동 경로
    [SerializeField] List<EnemyStats> enemyStats; // 몬스터 순서
    [SerializeField] float enemyStatWeight = 1; // 적 스탯 가중치
    float enemyStatWeightDelta = 1;

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
        // 스테이지에 따른 모델 교체
        ChangeEnemyModel(enemy);

        // 적의 능력치 및 상태 초기화
        enemy.GetComponent<Enemy>().Init(this, currentEnemyStat, enemyStatWeightDelta, wayPoint);

        spawnCountDelta--; // 스폰 카운트 감소
        enemy.SetActive(true); // 적 활성화
    }

    private void ChangeEnemyModel(GameObject enemy)
    {
        // 스테이지에 따른 적 모델 변경 (스테이지에 맞는 모델 적용)
        int modelIndex = stage % enemyModels.Count;
        GameObject newModel = enemyModels[modelIndex];

        // 기존 모델 삭제 후 새로운 모델을 자식으로 추가
        foreach (Transform child in enemy.transform)
        {
            Destroy(child.gameObject); // 이전 모델 삭제
        }
        Instantiate(newModel, enemy.transform); // 새로운 모델 생성
    }

    public void OnReleaseEnemy(GameObject enemy)
    {
        enemy.SetActive(false); // 적 비활성화
        deadEnemyCount++;
        remainEnemyText.text = (maxSpawnCount - deadEnemyCount).ToString();

        // 모든 적이 사망한 경우
        if (deadEnemyCount == maxSpawnCount)
        {
            CancelInvoke(nameof(SpawnEnemy));
            battlePhase.ChangePhase();
            // 전투가 끝난 후 다음 스테이지
            stage++;
            waveText.text = $"Wave {stage.ToString()}";
        }
    }

    void DestroyEnemy(GameObject enemy)
    {
        Destroy(enemy);
    }
    #endregion
}