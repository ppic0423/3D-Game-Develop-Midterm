using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MinePool : PoolBase
{
    #region Singleton
    private static MinePool _instance;
    public static MinePool Instance
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
