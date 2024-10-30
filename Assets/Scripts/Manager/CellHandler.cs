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
        // 타워 있을 경우
        if (target.GetComponent<Tile>().turret != null)
        {
            // UI 활성화
            onTower_UI.gameObject.SetActive(true);
            current_UI = onTower_UI;

            // 레벨
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

            // 판매 가격
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

    // 터렛 생성
    public void BuildTurret(GameObject prefab)
    {
        // 골드 확인
        int cost = prefab.GetComponent<Turret>().Cost;
        if (ResourceManager.Instance.Gold < cost)
            return;
        // 골드 사용
        ResourceManager.Instance.UseGold(cost);

        // 터렛 생성
        GameObject go = Instantiate(prefab, target.transform);
        Turret newTurret = go.GetComponent<Turret>();
        target.GetComponent<Tile>().turret = newTurret;
        SoundManager.Instance.PlaySound(_buildSound);

        /* 시너지 적용 */

        // 주변 타일 및 터렛 확인
        List<Tile> neighbourTiles = target.GetComponent<Tile>().parentGrid.Neighbours(target.GetComponent<Tile>());
        // 방금 만든 터렛 시너지 적용할 대상 수집
        List<Turret> nearbyTurretsForNewTurret = new List<Turret>();

        // 방금 만든 터렛에 시너지 적용
        SynergyManager.Instance.CheckAndApplySynergy(newTurret, nearbyTurretsForNewTurret);
        
        // 주변 터렛 시너지 적용
        foreach (Tile tile in neighbourTiles)
        {
            if (tile.turret != null)
            {
                nearbyTurretsForNewTurret.Add(tile.turret);

                // 이웃 타일의 터렛 시너지를 적용할 대상 수집
                List<Turret> neighbourTurretsForExistingTurret = new List<Turret>();
                foreach (Tile neighbourTile in target.GetComponent<Tile>().parentGrid.Neighbours(tile))
                {
                    if (neighbourTile.turret != null)
                    {
                        neighbourTurretsForExistingTurret.Add(neighbourTile.turret);
                    }
                }

                // 이웃 터렛 시너지 적용
                SynergyManager.Instance.CheckAndApplySynergy(tile.turret, neighbourTurretsForExistingTurret);
            }
        }
        // 마우스 입력 해제
        mouseInput.SetSelector(null);
    }
    // 터렛 업그레이드
    public void UpgradeTurret()
    {
        Turret targetTurret = target.GetComponent<Tile>().turret;
        targetTurret.Upgrade();
        SoundManager.Instance.PlaySound(_sellSound);

        mouseInput.SetSelector(null);

    }
    // 터렛 판매
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