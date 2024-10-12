using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadePool : PoolBase
{
    #region Singleton
    private static GrenadePool _instance;
    public static GrenadePool Instance
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
