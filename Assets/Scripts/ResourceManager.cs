using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    #region Singleton
    private static ResourceManager _instance;
    public static ResourceManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("@ResourceManager");
                _instance = go.AddComponent<ResourceManager>();
            }
            DontDestroyOnLoad(_instance);
            return _instance;
        }
    }
    #endregion
    [SerializeField] int hp;
    [SerializeField] int gold;

    public void AddGold(int value)
    {
        gold += value;
    }
    public void UseGold(int value)
    {
        gold -= value;
    }
    public void TakeDamage(int damage)
    {
        hp -= damage;
        UnityEngine.Debug.Log(hp);

        if(hp <= 0)
        {
            // TODO. »ç¸Á Ã³¸®
        }
    }
}
