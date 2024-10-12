using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            Enemy[] enemys = FindObjectsOfType<Enemy>();

            foreach(Enemy enemy in enemys)
            {
                enemy.TakeDamage(999);
            }
        }
    }
}
