using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)) 
        {
            Time.timeScale = 0;
        }
        else if(Input.GetKeyDown(KeyCode.Q)) 
        { 
            Time.timeScale = 1;
        }
        else if(Input.GetKeyDown (KeyCode.W)) 
        {
            Time.timeScale = 5;
        }
    }
}
