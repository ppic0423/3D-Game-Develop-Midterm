using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] BattlePhase battlePhase;

    [Header("���� ����")]
    [SerializeField] float spawnInterval = 1f; // ��ȯ �ð� ��
    [SerializeField] int maxSpawnCount = 10; // �ִ� ��ȯ ����
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
        // ���� �ʱ�ȭ
        spawnCountDelta = maxSpawnCount;
        deadEnemyCount = 0;
        remainEnemyText.text = $"{maxSpawnCount - deadEnemyCount}/{maxSpawnCount}";
        waveText.text = $"Wave {stage.ToString()}";

        // ����Ŭ�� ���� ������ ���
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

        // �� ��ȯ ����
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
    ObjectPool<GameObject> pool; // ������Ʈ Ǯ

    [Header("���� ����")]
    [SerializeField] GameObject enemyPrefab; // �⺻ ���� ������
    [SerializeField] List<GameObject> enemyModels; // ���������� ���� �� ����Ʈ
    [SerializeField] WayPoint wayPoint; // �̵� ���
    [SerializeField] List<EnemyStats> enemyStats; // ���� ����
    [SerializeField] float enemyStatWeight = 1; // �� ���� ����ġ
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
        // ���������� ���� �� ��ü
        ChangeEnemyModel(enemy);

        // ���� �ɷ�ġ �� ���� �ʱ�ȭ
        enemy.GetComponent<Enemy>().Init(this, currentEnemyStat, enemyStatWeightDelta, wayPoint);

        spawnCountDelta--; // ���� ī��Ʈ ����
        enemy.SetActive(true); // �� Ȱ��ȭ
    }

    private void ChangeEnemyModel(GameObject enemy)
    {
        // ���������� ���� �� �� ���� (���������� �´� �� ����)
        int modelIndex = (stage % enemyModels.Count) - 1;
        GameObject newModel = enemyModels[modelIndex];

        // ���� �� ���� �� ���ο� ���� �ڽ����� �߰�
        foreach (Transform child in enemy.transform)
        {
            Destroy(child.gameObject); // ���� �� ����
        }
        Instantiate(newModel, enemy.transform); // ���ο� �� ����
    }

    public void OnReleaseEnemy(GameObject enemy)
    {
        enemy.SetActive(false); // �� ��Ȱ��ȭ
        deadEnemyCount++;
        remainEnemyText.text = $"{maxSpawnCount - deadEnemyCount}/{maxSpawnCount}";

        // ��� ���� ����� ���
        if (deadEnemyCount == maxSpawnCount)
        {
            CancelInvoke(nameof(SpawnEnemy));
            battlePhase.ChangePhase();
            // ������ ���� �� ���� ��������
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