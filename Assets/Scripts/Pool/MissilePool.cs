using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissilePool : PoolBase
{
    #region Singleton
    private static MissilePool _instance;
    public static MissilePool Instance
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
