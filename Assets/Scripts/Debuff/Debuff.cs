using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Debuff
{
    public Enemy enemy;
    [SerializeField] public float duration;
    
    public Debuff(float duration)
    {
        this.duration = duration;
    }

    public virtual void Apply()
    {
        enemy.Debuffs.Add(this);
    }
    public virtual void Remove()
    {
        enemy.Debuffs.Remove(this);
    }

    public virtual IEnumerator StartDebuff()
    {
        Apply();
        yield return new WaitForSeconds(duration);
        Remove();
    }
}
