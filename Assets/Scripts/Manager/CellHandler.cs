using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class CellHandler : Selector
{
    [Header("UI")]
    RectTransform current_UI;
    [SerializeField] RectTransform onTower_UI;
    [SerializeField] RectTransform empty_UI;
    [SerializeField] GameObject TurretShop_1;
    [SerializeField] GameObject TurretShop_2;

    [Header("OnTower UI")]
    [SerializeField] TextMeshProUGUI reinforcePrice;
    [SerializeField] TextMeshProUGUI level_UI;
    [SerializeField]TextMeshProUGUI sellPrice;
    [SerializeField] AudioClip _buildSound;
    [SerializeField] AudioClip _sellSound;
    [SerializeField] Sprite[] synergy_Sprites;
    [SerializeField] Image[] synergy_Images;
    
    public override void Enter()
    {
        // Ÿ�� ���� ���
        if (target.GetComponent<Tile>().turret != null)
        {
            // UI Ȱ��ȭ
            onTower_UI.gameObject.SetActive(true);
            current_UI = onTower_UI;

            // ����
            Turret turret = target.GetComponent<Tile>().turret;
            if(turret.currentLevel == 2)
            {
                level_UI.text = "LV\nMAX";
                reinforcePrice.text = "-";
            }
            else
            {
                level_UI.text = $"LV\n{turret.currentLevel}";
                reinforcePrice.text = (turret.upgrades[turret.currentLevel].cost).ToString();
            }

            // �Ǹ� ����
            sellPrice.text = (turret.Cost * 2 / 3).ToString();

            for(int i = 0; i < 3; i++)
            {
                if(i <= target.GetComponent<Tile>().turret.synergys.Count)
                {
                    synergy_Images[i].sprite = synergy_Sprites[(int)target.GetComponent<Tile>().turret.synergys[i]];
                }
                else
                {
                    synergy_Images[i] = null;
                }
            }
        }
        else
        {
            empty_UI.gameObject.SetActive(true);
            TurretShop_1.SetActive(true);
            TurretShop_2.SetActive(false);
            current_UI = empty_UI;
        }
    }
    public override void Tick()
    {
        current_UI.transform.position = Camera.main.WorldToScreenPoint(target.transform.position);
    }
    public override void Exit()
    {
        onTower_UI.gameObject.SetActive(false);
        empty_UI.gameObject.SetActive(false);
    }

    // �ͷ� ����
    public void BuildTurret(GameObject prefab)
    {
        // ��� Ȯ��
        int cost = prefab.GetComponent<Turret>().Cost;
        if (ResourceManager.Instance.Gold < cost)
            return;
        // ��� ���
        ResourceManager.Instance.UseGold(cost);

        // �ͷ� ����
        GameObject go = Instantiate(prefab, target.transform);
        Turret newTurret = go.GetComponent<Turret>();
        target.GetComponent<Tile>().turret = newTurret;
        SoundManager.Instance.PlaySound(_buildSound);

        /* �ó��� ���� */

        // �ֺ� Ÿ�� �� �ͷ� Ȯ��
        List<Tile> neighbourTiles = target.GetComponent<Tile>().parentGrid.Neighbours(target.GetComponent<Tile>());
        // ��� ���� �ͷ� �ó��� ������ ��� ����
        List<Turret> nearbyTurretsForNewTurret = new List<Turret>();

        // ��� ���� �ͷ��� �ó��� ����
        SynergyManager.Instance.CheckAndApplySynergy(newTurret, nearbyTurretsForNewTurret);
        
        // �ֺ� �ͷ� �ó��� ����
        foreach (Tile tile in neighbourTiles)
        {
            if (tile.turret != null)
            {
                nearbyTurretsForNewTurret.Add(tile.turret);

                // �̿� Ÿ���� �ͷ� �ó����� ������ ��� ����
                List<Turret> neighbourTurretsForExistingTurret = new List<Turret>();
                foreach (Tile neighbourTile in target.GetComponent<Tile>().parentGrid.Neighbours(tile))
                {
                    if (neighbourTile.turret != null)
                    {
                        neighbourTurretsForExistingTurret.Add(neighbourTile.turret);
                    }
                }

                // �̿� �ͷ� �ó��� ����
                SynergyManager.Instance.CheckAndApplySynergy(tile.turret, neighbourTurretsForExistingTurret);
            }
        }
        // ���콺 �Է� ����
        mouseInput.SetSelector(null);
    }
    // �ͷ� ���׷��̵�
    public void UpgradeTurret()
    {
        Turret targetTurret = target.GetComponent<Tile>().turret;
        targetTurret.Upgrade();
        SoundManager.Instance.PlaySound(_sellSound);

        mouseInput.SetSelector(null);

    }
    // �ͷ� �Ǹ�
    public void SellTurret()
    {
        if (target.GetComponent<Tile>().turret == null)
            return;

        Turret targetTurret = target.GetComponent<Tile>().turret;
        Destroy(targetTurret.gameObject);
        target.GetComponent<Tile>().turret = null;

        ResourceManager.Instance.AddGold((int)(targetTurret.Cost * 2 / 3));
        SoundManager.Instance.PlaySound(_sellSound);

        mouseInput.SetSelector(null);
    }
}