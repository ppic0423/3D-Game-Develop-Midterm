using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class PoolBase : MonoBehaviour
{
    public ObjectPool<GameObject> pool;
    [SerializeField] protected GameObject Prefab;  // ��ӹ��� Ŭ�������� �������� �Ҵ�

    private void Start()
    {
        pool = new ObjectPool<GameObject>(
        createFunc: CreateObject,
        actionOnGet: OnGetObject,
        actionOnRelease: OnReleaseObject,
        actionOnDestroy: DestroyObject,
        defaultCapacity: 10
        );
    }

    GameObject CreateObject()
    {
        GameObject bullet = Instantiate(Prefab, transform.position, Quaternion.identity);
        bullet.transform.parent = this.transform;
        bullet.SetActive(false);
        return bullet;
    }

    void OnGetObject(GameObject @object)
    {
        @object.SetActive(true); // �Ѿ� Ȱ��ȭ
    }

    void OnReleaseObject(GameObject @object)
    {
        @object.SetActive(false); // �Ѿ� ��Ȱ��ȭ
    }

    void DestroyObject(GameObject @object)
    {
        Destroy(@object);  // ������Ʈ �ı�
    }
}