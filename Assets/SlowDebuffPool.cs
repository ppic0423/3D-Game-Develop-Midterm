using UnityEngine;

public class SlowDebuffPool : PoolBase
{
    #region Singleton
    private static SlowDebuffPool _instance;
    public static SlowDebuffPool Instance
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
