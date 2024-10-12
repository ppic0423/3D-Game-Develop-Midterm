using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CommonBulletPool : PoolBase
{
    #region Singleton
    private static CommonBulletPool _instance;
    public static CommonBulletPool Instance
    {
        get
        {
            return _instance;
        }
    }
    #endregion

    private void Awake()
    {
        _instance = this;
    }
}
