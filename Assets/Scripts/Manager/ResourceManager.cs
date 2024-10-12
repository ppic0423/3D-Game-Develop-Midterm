using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceManager : MonoBehaviour
{
    #region Singleton
    private static ResourceManager _instance;
    public static ResourceManager Instance
    {
        get
        {
            return _instance;
        }
    }
    #endregion
    [SerializeField] int hp;
    public int Gold
    {
        get { return gold; }
        set { gold = value; }
    }
    [SerializeField] int gold = 0;
    [SerializeField] TextMeshProUGUI hp_text;
    [SerializeField] TextMeshProUGUI gold_text;

    private void Awake()
    {
        _instance = this;
        hp_text.text = hp.ToString();
        gold_text.text = gold.ToString();
    }

    public void AddGold(int value)
    {
        gold += value;
        gold_text.text = gold.ToString();
    }
    public void UseGold(int value)
    {
        gold -= value;
        gold_text.text = gold.ToString();
    }
    public void TakeDamage(int damage)
    {
        hp -= damage;
        hp_text.text = hp.ToString();

        if (hp <= 0)
        {
            // TODO. »ç¸Á Ã³¸®
        }
    }
}
