using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class PoolBase : MonoBehaviour
{
    public ObjectPool<GameObject> pool;
    [SerializeField] protected GameObject Prefab;  // 상속받은 클래스에서 프리팹을 할당

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
        @object.SetActive(true); // 총알 활성화
    }

    void OnReleaseObject(GameObject @object)
    {
        @object.SetActive(false); // 총알 비활성화
    }

    void DestroyObject(GameObject @object)
    {
        Destroy(@object);  // 오브젝트 파괴
    }
}