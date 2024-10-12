using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roll_Button : MonoBehaviour
{
    [SerializeField] GameObject TurretShop_Button_1;
    [SerializeField] GameObject TurretShop_Button_2;

    public void OnClick()
    {
        if(TurretShop_Button_1.activeSelf) 
        {
            TurretShop_Button_1.SetActive(false);
            TurretShop_Button_2.SetActive(true);
        }
        else if(TurretShop_Button_2.activeSelf)
        {
            TurretShop_Button_1.SetActive(true);
            TurretShop_Button_2.SetActive(false);
        }
    }
}
