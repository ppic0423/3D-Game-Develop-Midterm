using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaknessBulletPool : PoolBase
{
    #region Singleton
    private static WeaknessBulletPool _instance;
    public static WeaknessBulletPool Instance
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
