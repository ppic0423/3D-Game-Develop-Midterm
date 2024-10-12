using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowBulletPool : PoolBase
{
    #region Singleton
    private static SlowBulletPool _instance;
    public static SlowBulletPool Instance
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
